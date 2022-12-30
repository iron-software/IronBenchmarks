using IronBenchmarks.Reporting.Configuration;
using IronBenchmarks.Reporting.ReportData;
using IronXL;
using IronXL.Formatting;

namespace IronBenchmarks.Reporting.Tests
{
    public class ReportGeneratorTests : TestsBase
    {
        private static readonly Dictionary<int, string> letters = new()
        {
            {1, "A"},
            {2, "B"},
            {3, "C"},
            {4, "D"},
            {5, "E"},
            {6, "F"},
            {7, "G"},
            {8, "H"},
            {9, "I"},
            {10, "J"},
            {11, "K"},
            {12, "L"},
            {13, "M"},
            {14, "N"},
            {15, "O"},
            {16, "P"},
            {17, "Q"},
            {18, "R"},
            {19, "S"},
            {20, "T"},
            {21, "U"},
            {22, "V"},
            {23, "W"},
            {24, "X"},
            {25, "Y"},
            {26, "Z"}
        };

        [Fact]
        public void GenerateReport_FromBenchmarkData_ReportCreatedCorrectly()
        {
            // Arrange
            var reportConfig = new ReportingConfig();
            var reportGenerator = new ReportGenerator(reportConfig);
            var contenders = new string[] { "IXL", "IXLO", "Asp", "ClXl", "Npoi", "Epp" };
            var contendersToAppend = new string[] { "Interop" };
            var finalListOfContenders = contenders.Concat(contendersToAppend).ToArray();
            var benchNames = new string[] { "bench0", "bench1", "bench2", "bench3", "bench4", "bench5", "bench6", "bench7", "bench8", "bench9", "bench10", };
            var benchNamesToAppend = new string[] { "bench0", "bench1", "bench2", "bench3", "bench4", "bench5", "bench6", "bench7", "bench8", "bench9" };
            var benchmarksData = new List<BenchmarkData>()
            {
                CreateBenchmarkData(ReportDataType.MeanTime, contenders, benchNames),
                CreateBenchmarkData(ReportDataType.MemoryAlloc, contenders, benchNames)
            };
            var benchmarksDataToAppend = new List<BenchmarkData>()
            {
                CreateBenchmarkData(ReportDataType.MeanTime, contendersToAppend, benchNamesToAppend),
                CreateBenchmarkData(ReportDataType.MemoryAlloc, contendersToAppend, benchNamesToAppend)
            };

            // Act
            var reportName = reportGenerator.GenerateReport(benchmarksData, "ExcelLibsTests");
            reportConfig.AppendToLastReport = true;
            var appendedReportName = reportGenerator.GenerateReport(benchmarksDataToAppend, "ExcelLibsTests");

            // Assert
            var report = WorkBook.Load(reportName);
            var appendedReport = WorkBook.Load(appendedReportName);

            Assert.Equal(benchmarksData.Count, report.WorkSheets.Count);
            Assert.Equal("Performance", report.WorkSheets[0].Name);
            Assert.Equal("Memory Allocation", report.WorkSheets[1].Name);
            Assert.Equal(benchmarksData.Count, appendedReport.WorkSheets.Count);
            Assert.Equal("Performance", appendedReport.WorkSheets[0].Name);
            Assert.Equal("Memory Allocation", appendedReport.WorkSheets[1].Name);

            reportConfig.AppendToLastReport = false;
            foreach (var sheet in report.WorkSheets)
            {
                Assert.Equal(benchNames.Length, sheet.Charts.Count);
                AssertDataPlacedFormattedCorrectly(sheet, reportConfig, contenders, benchNames);
                AssertChartsPlacedCorrectly(sheet, reportConfig, contenders.Length);
            }

            reportConfig.AppendToLastReport = true;
            foreach (var sheet in appendedReport.WorkSheets)
            {
                Assert.Equal(benchNames.Length, sheet.Charts.Count);
                AssertDataPlacedFormattedCorrectly(sheet, reportConfig, finalListOfContenders, benchNames);
                AssertChartsPlacedCorrectly(sheet, reportConfig, finalListOfContenders.Length);
            }
        }

        private static BenchmarkData CreateBenchmarkData(ReportDataType dataType, string[] contenders, string[] benchNames)
        {
            var data = new BenchmarkData(dataType);

            for (var i = 0; i < contenders.Length; i++)
            {
                var entry = new BenchmarkDataEntry(contenders[i]);

                for (var j = 0; j < benchNames.Length; j++)
                {
                    entry.Add(benchNames[j], dataType == ReportDataType.MeanTime ? Units.us : Units.KB, j + 1);
                }

                data.DataEntries.Add(entry);
            }

            return data;
        }

        private static void AssertDataPlacedFormattedCorrectly(WorkSheet sheet, ReportingConfig reportConfig, string[] contenders, string[] benchNames)
        {
            for (var i = 0; i < contenders.Length; i++)
            {
                Assert.Equal(contenders[i], sheet[$"A{reportConfig.DataTableStartingRow + i + 1}"].Value);
            }

            for (var i = 0; i < benchNames.Length; i++)
            {
                var name = sheet.Name == "Performance"
                    ? $"{benchNames[i]}, {EnumHelper.GetEnumDescription(Units.us)}"
                    : $"{benchNames[i]}, {EnumHelper.GetEnumDescription(Units.KB)}";

                Assert.Equal(name, sheet[$"{letters[i + 2]}{reportConfig.DataTableStartingRow}"].Value);
            }

            for (var i = 0; i < contenders.Length; i++)
            {
                for (var j = 0; j < benchNames.Length; j++)
                {
                    var cell = sheet[$"{letters[j + 2]}{reportConfig.DataTableStartingRow + i + 1}"];

                    if (reportConfig.AppendToLastReport && i == contenders.Length - 1 && j == benchNames.Length - 1)
                    {
                        Assert.Equal(0.0, cell.Value);
                    } 
                    else
                    {
                        Assert.Equal(j + 1.0, cell.Value);
                    }
                    
                    Assert.Equal(BuiltinFormats.Thousands2, cell.FormatString);
                }
            }
        }

        private static void AssertChartsPlacedCorrectly(WorkSheet sheet, IReportingConfig reportConfig, int contendersNum)
        {
            var chartsInRow = 0;
            var chartRow = 0;

            foreach (var chart in sheet.Charts)
            {
                var topRow = contendersNum + 1 + (reportConfig.ChartHeight * chartRow);
                var bottomRow = topRow + reportConfig.ChartHeight;
                var firstColumn = reportConfig.ChartWidth * chartsInRow;
                var lastColumn = firstColumn + reportConfig.ChartWidth;

                Assert.Equal(topRow, chart.Position.TopRowIndex);
                Assert.Equal(bottomRow, chart.Position.BottomRowIndex);
                Assert.Equal(firstColumn, chart.Position.LeftColumnIndex);
                Assert.Equal(lastColumn, chart.Position.RightColumnIndex);

                chartsInRow++;

                if (chartsInRow >= reportConfig.ChartsInRow)
                {
                    chartRow++;
                    chartsInRow = 0;
                }
            }
        }
    }
}