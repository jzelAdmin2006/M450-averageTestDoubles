using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Average.Test
{
    public class AverageTestStub
    {
        public static IEnumerable<object[]> SimpleStubTestCases()
        {
            yield return new object[] { new Func<Average, double>(average => average.ComputeMeanOfFile()), 3.0 };
            yield return new object[] { new Func<Average, double>(average => average.ComputeMedianOfFile()), 3.0 };
            yield return new object[] { new Func<Average, List<int>>(average => average.ComputeModeOfFile()), new[] { 1, 2, 3, 4, 5 } };
        }

        public static IEnumerable<object[]> FlexibleStubTestCases()
        {
            yield return new object[] { new[] { 1, 2, 3, 4, 5 }, new Func<Average, double>(average => average.ComputeMeanOfFile()), 3.0 };
            yield return new object[] { new[] { 1, 2, 3, 4 }, new Func<Average, double>(average => average.ComputeMedianOfFile()), 2.5 };
            yield return new object[] { new[] { 2, 5, 6, 2, 5 }, new Func<Average, List<int>>(average => average.ComputeModeOfFile()), new[] { 2, 5 } };
        }

        [Theory]
        [MemberData(nameof(SimpleStubTestCases))]
        public void TestAverageCalculationsSimple<T>(Func<Average, T> toTest, object expected)
        {
            Average average = new(new StubFileAccess());

            object actual = toTest(average);

            Assert.Equal(expected, actual);
        }

        [Theory]
        [MemberData(nameof(FlexibleStubTestCases))]
        public void TestAverageCalculationsFlexible<T>(int[] input, Func<Average, T> toTest, object expected)
        {
            Average average = new(new FlexibleStubFileAccess(input.ToList()));

            object actual = toTest(average);

            Assert.Equal(expected, actual);
        }

        internal class StubFileAccess : IFileOperations
        {
            public List<int> ReadNumbers()
            {
                return new int[] { 1, 2, 3, 4, 5 }.ToList();
            }
        }

        // In my opinion, this is still a stub, because it is a hard-coded list of numbers but per test case.
        internal class FlexibleStubFileAccess : IFileOperations
        {
            private readonly List<int> numbers;

            public FlexibleStubFileAccess(List<int> numbers)
            {
                this.numbers = numbers;
            }

            public List<int> ReadNumbers()
            {
                return numbers;
            }
        }
    }
}
