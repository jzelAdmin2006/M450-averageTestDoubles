using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Average.Test
{
    public class AverageTest
    {
        private static Average FromFilePath(string filePath)
        {
            return new(new FakeFileAccess(filePath));
        }

        public static IEnumerable<object[]> EmptyFileTestCases()
        {
            yield return new object[] { new Func<Average, double>(average => average.ComputeMeanOfFile()) };
            yield return new object[] { new Func<Average, double>(average => average.ComputeMedianOfFile()) };
            yield return new object[] { new Func<Average, List<int>>(average => average.ComputeModeOfFile()) };
        }

        [Theory]
        [MemberData(nameof(EmptyFileTestCases))]
        void TestCalculateWithEmptyFiles<T>(Func<Average, T> toTest)
        {
            Average average = FromFilePath("/path/to/an/empty/file");

            var exception = Assert.Throws<ArgumentException>(() => toTest(average));
            Assert.Contains("empty", exception.Message);
        }

        [Theory]
        [InlineData("/path/to/some/other/file", 3, 0)]
        [InlineData("C:\\test-data\\third-file.txt", 14.333, 0.001)]
        [InlineData("/path/to/fourth-file", 5.8, 0)]
        public void TestCalculateMean(string filePath, double expected, double tolerance)
        {
            Average average = FromFilePath(filePath);

            var actual = average.ComputeMeanOfFile();

            Assert.True(Math.Abs(expected - actual) <= tolerance, $"Expected: {expected}, Actual: {actual}, Tolerance: {tolerance}");
        }

        [Theory]
        [InlineData("/path/to/some/other/file", 3)]
        [InlineData("C:\\test-data\\third-file.txt", 7)]
        [InlineData("/path/to/fourth-file", 6)]
        public void TestCalculateMedian(string filePath, double expected)
        {
            Average average = FromFilePath(filePath);

            var actual = average.ComputeMedianOfFile();

            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("/path/to/some/other/file", new int[] { 1, 2, 3, 4, 5 })]
        [InlineData("C:\\test-data\\third-file.txt", new int[] { 7, 34, 2 })]
        [InlineData("/path/to/fourth-file", new int[] { -3 })]
        public void TestCalculateMode(string filePath, int[] expected)
        {
            Average average = FromFilePath(filePath);

            var actual = average.ComputeModeOfFile();

            Assert.Equal(expected, actual);
        }
    }

    internal class FakeFileAccess : IFileOperations
    {
        private Dictionary<string, List<int>> _pseudoFiles = new() {
            { "/path/to/an/empty/file", new() },
            { "/path/to/some/other/file", new() { 1, 2, 3, 4, 5 } },
            { "C:\\test-data\\third-file.txt", new() { 7, 34, 2 } },
            { "/path/to/fourth-file", new() { 14, 15, 6, -3, -3 } }
        };
        private string _pseudoPath;

        internal FakeFileAccess(string path)
        {
            _pseudoPath = path;
        }

        public List<int> ReadNumbers()
        {
            return _pseudoFiles[_pseudoPath];
        }
    }
}
