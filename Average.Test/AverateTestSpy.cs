using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Average.Test
{
    public class AverateTestSpy
    {
        [Fact]
        public void TestComputeMeanOfFile()
        {
            string tempFilePath = Path.Combine(Path.GetTempPath(), "numbers.txt");
            StreamWriter writer = new(tempFilePath);
            writer.WriteLine("1");
            writer.WriteLine("2");
            writer.WriteLine("3");
            writer.Close();
            SpyFileAccess spyFileAccess = new(new FileAccess(tempFilePath));
            Average average = new(spyFileAccess);

            double actual = average.ComputeMeanOfFile();

            Assert.Equal(2, actual);
            Assert.Single(spyFileAccess.ReadNumberResults);
            Assert.Equal(new[] { 1, 2, 3 }, spyFileAccess.ReadNumberResults[0]);

            File.Delete(tempFilePath);
        }

        internal class SpyFileAccess : IFileOperations
        {
            private readonly FileAccess fileAccess;
            private readonly List<List<int>> readNumberResults = new();
            public ReadOnlyCollection<ReadOnlyCollection<int>> ReadNumberResults
            {
                get { return readNumberResults.Select(list => list.AsReadOnly()).ToList().AsReadOnly(); }
            }

            internal SpyFileAccess(FileAccess fileAccess)
            {
                this.fileAccess = fileAccess;
            }

            public List<int> ReadNumbers()
            {
                List<int> readNumbers = fileAccess.ReadNumbers();
                readNumberResults.Add(readNumbers);
                return readNumbers;
            }
        }
    }
}
