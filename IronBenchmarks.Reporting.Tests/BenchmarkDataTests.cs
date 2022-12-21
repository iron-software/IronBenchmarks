using BenchmarkDotNet.Reports;

namespace IronBenchmarks.Reporting.Tests
{
    public class BenchmarkDataTests
    {
        [Fact]
        public void BenchmarkData_Constructor_WithSummaries_InitializesDataTypeAndDataEntries()
        {
            // Arrange
            var summaries = new List<Summary>();
            var dataType = ReportDataType.MeanTime;

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
            var dataType = ReportDataType.MeanTime;

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
            var expectedNames = new string[] { "Test1", "Test2", "Test3" };
            var dataEntry = new BenchmarkDataEntry("Test");
            benchmarkData.DataEntries.Add(dataEntry);

            // Add some data entries with the expected names
            foreach (var name in expectedNames)
            {
                dataEntry.Add(name, 1.0);
            }
            
            // Act
            var actualNames = benchmarkData.GetBenchmarkNames();

            // Assert
            Assert.Equal(expectedNames, actualNames);
        }

        [Fact]
        public void BenchmarkDataEntry_Indexer_ReturnsCorrectValue()
        {
            // Arrange
            var dataEntry = new BenchmarkDataEntry("Test");
            dataEntry.Add("Test1", 123.45);

            // Act
            var value = dataEntry["Test1"];

            // Assert
            Assert.Equal(123.45, value);
        }
    }
}
