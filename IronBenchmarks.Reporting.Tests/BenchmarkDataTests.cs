using BenchmarkDotNet.Reports;
using IronBenchmarks.Reporting.ReportData;

namespace IronBenchmarks.Reporting.Tests
{
    public class BenchmarkDataTests
    {
        [Fact]
        public void BenchmarkData_Constructor_WithSummaries_InitializesDataTypeAndDataEntries()
        {
            // Arrange
            var summaries = new List<Summary>();
            ReportDataType dataType = ReportDataType.MeanTime;

            // Act
            var benchmarkData = new BenchmarkData(summaries, dataType);

            // Assert
            Assert.Equal(dataType, benchmarkData.DataType);
            Assert.NotNull(benchmarkData.DataEntries);
            Assert.Empty(benchmarkData.DataEntries);
        }

        [Fact]
        public void BenchmarkData_Constructor_WithDataType_InitializesDataType()
        {
            // Arrange
            ReportDataType dataType = ReportDataType.MeanTime;

            // Act
            var benchmarkData = new BenchmarkData(dataType);

            // Assert
            Assert.Equal(dataType, benchmarkData.DataType);
        }

        [Fact]
        public void BenchmarkData_GetBenchmarkNames_ReturnsCorrectNames()
        {
            // Arrange
            var benchmarkData = new BenchmarkData(ReportDataType.MeanTime);
            string[] expectedNames = new string[] { "Test1", "Test2", "Test3" };
            var dataEntry = new BenchmarkDataEntry("Test");
            benchmarkData.DataEntries.Add(dataEntry);

            // Add some data entries with the expected names
            foreach (string name in expectedNames)
            {
                dataEntry.Add(name, Units.s, 1.0);
            }

            // Act
            Dictionary<string, Units> actualNames = benchmarkData.GetBenchmarkNames();

            // Assert
            Assert.Equal(expectedNames, actualNames.Keys.ToArray());
        }

        [Fact]
        public void BenchmarkDataEntry_Indexer_ReturnsCorrectValue()
        {
            // Arrange
            var dataEntry = new BenchmarkDataEntry("Test");
            dataEntry.Add("Test1", Units.s, 123.45);

            // Act
            BenchmarkDataPoint value = dataEntry["Test1"];

            // Assert
            Assert.Equal(123.45, value.Value);
        }

        [Fact]
        public void GetBenchmarkNames_ShouldReturnCorrectBenchmarkNames_WhenCalled()
        {
            // Arrange
            var benchmarkData = new BenchmarkData(ReportDataType.MeanTime);
            var benchmarkDataEntry = new BenchmarkDataEntry("Contender 1");
            benchmarkDataEntry.Add("Benchmark 1", Units.s, 0);
            benchmarkDataEntry.Add("Benchmark 2", Units.us, 0);
            benchmarkDataEntry.Add("Benchmark 3", Units.ms, 0);
            benchmarkData.DataEntries.Add(benchmarkDataEntry);

            // Act
            Dictionary<string, Units> result = benchmarkData.GetBenchmarkNames();

            // Assert
            Assert.Equal(new Dictionary<string, Units> { { "Benchmark 1", Units.s }, { "Benchmark 2", Units.us }, { "Benchmark 3", Units.ms } }, result);
        }

        [Fact]
        public void GetNumberOfContenders_ShouldReturnCorrectNumberOfContenders_WhenCalled()
        {
            // Arrange
            var benchmarkData = new BenchmarkData(ReportDataType.MeanTime);
            benchmarkData.DataEntries.Add(new BenchmarkDataEntry("Contender 1"));
            benchmarkData.DataEntries.Add(new BenchmarkDataEntry("Contender 2"));
            benchmarkData.DataEntries.Add(new BenchmarkDataEntry("Contender 3"));

            // Act
            int result = benchmarkData.GetNumberOfContenders();

            // Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void GetNumberOfBenchmarks_ShouldReturnCorrectNumberOfBenchmarks_WhenCalled()
        {
            // Arrange
            var benchmarkData = new BenchmarkData(ReportDataType.MeanTime);
            var benchmarkDataEntry = new BenchmarkDataEntry("Contender 1");
            benchmarkDataEntry.Add("Benchmark 1", Units.s, 0);
            benchmarkDataEntry.Add("Benchmark 2", Units.us, 0);
            benchmarkDataEntry.Add("Benchmark 3", Units.ms, 0);
            benchmarkData.DataEntries.Add(benchmarkDataEntry);

            // Act
            int result = benchmarkData.GetNumberOfBenchmarks();

            // Assert
            Assert.Equal(3, result);
        }
    }
}
