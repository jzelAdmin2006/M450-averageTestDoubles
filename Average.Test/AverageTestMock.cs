using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Average.Test
{
    public class AverageTestMock
    {
        public static IEnumerable<object[]> TestCases()
        {
            yield return new object[] { new Func<Average, double>(average => average.ComputeMeanOfFile()), 3.0 };
            yield return new object[] { new Func<Average, double>(average => average.ComputeMedianOfFile()), 3.0 };
            yield return new object[] { new Func<Average, List<int>>(average => average.ComputeModeOfFile()), new[] { 1, 2, 3, 4, 5 } };
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        public void TestAverageCalculations<T>(Func<Average, T> toTest, object expected)
        {
            MockFileAccess mockFileAccess = new();
            Average average = new(mockFileAccess);

            object actual = toTest(average);

            Assert.Equal(expected, actual);
            Assert.Equal(1, mockFileAccess.ReadNumbersCalls);
        }

        internal class MockFileAccess : IFileOperations
        {
            public int ReadNumbersCalls { get; private set; } = 0;

            public List<int> ReadNumbers()
            {
                ReadNumbersCalls++;
                return new int[] { 1, 2, 3, 4, 5 }.ToList();
            }
        }
    }
}
