using IronBenchmarks.Reporting.Configuration;
using IronXL;

namespace IronBenchmarks.Reporting.Tests
{
    public class ReportGeneratorTests : TestsBase
    {
        [Fact]
        public void GenerateReport_FromBenchmarkData_ReportCreatedCorrectly()
        {
            // Arrange
            var reportConfig = new ReportingConfig
            {
                ReportsFolder = "Reports",
                ChartWidth = 6,
                ChartHeight = 18,
                DataTableStartingRow = 20
            };
            var contender1 = "IXL";
            var contender2 = "IXLO";
            var benchName1 = "bench1";
            var benchName2 = "bench2";
            var cont1bench1 = 1.0;
            var cont1bench2 = 2.0;
            var cont2bench1 = 3.0;
            var cont2bench2 = 4.0;
            var reportGenerator = new ReportGenerator(reportConfig);
            var ixlDataEntry = new BenchmarkDataEntry(contender1);
            ixlDataEntry.Add(benchName1, cont1bench1);
            ixlDataEntry.Add(benchName2, cont1bench2);
            var ixloDataEntry = new BenchmarkDataEntry(contender2);
            ixloDataEntry.Add(benchName1, cont2bench1);
            ixloDataEntry.Add(benchName2, cont2bench2);
            var mean = new BenchmarkData(ReportDataType.MeanTime);
            mean.DataEntries.Add(ixlDataEntry);
            mean.DataEntries.Add(ixloDataEntry);
            var mem = new BenchmarkData(ReportDataType.MemoryAlloc);
            mem.DataEntries.Add(ixlDataEntry);
            mem.DataEntries.Add(ixloDataEntry);
            var benchmarksData = new List<BenchmarkData>() { mean, mem };

            // Act
            var reportName = reportGenerator.GenerateReport(benchmarksData, "ExcelLibsTests");

            // Assert
            var workbook = WorkBook.Load(reportName);

            Assert.Equal(benchmarksData.Count, workbook.WorkSheets.Count);
            Assert.Equal("Performance", workbook.WorkSheets[0].Name);
            Assert.Equal("Memory Allocation", workbook.WorkSheets[1].Name);

            foreach (var sheet in workbook.WorkSheets)
            {
                Assert.Equal(benchmarksData.Count, sheet.Charts.Count);
                Assert.Equal(contender1, sheet[$"A{reportConfig.DataTableStartingRow + 1}"].Value);
                Assert.Equal(contender2, sheet[$"A{reportConfig.DataTableStartingRow + 2}"].Value);
                Assert.Equal(benchName1, sheet[$"B{reportConfig.DataTableStartingRow}"].Value);
                Assert.Equal(benchName2, sheet[$"C{reportConfig.DataTableStartingRow}"].Value);
                Assert.Equal(cont1bench1, sheet[$"B{reportConfig.DataTableStartingRow + 1}"].Value);
                Assert.Equal(cont1bench2, sheet[$"C{reportConfig.DataTableStartingRow + 1}"].Value);
                Assert.Equal(cont2bench1, sheet[$"B{reportConfig.DataTableStartingRow + 2}"].Value);
                Assert.Equal(cont2bench2, sheet[$"C{reportConfig.DataTableStartingRow + 2}"].Value);
            }
        }
    }
}