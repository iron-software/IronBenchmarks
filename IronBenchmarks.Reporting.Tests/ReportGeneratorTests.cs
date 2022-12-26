using IronBenchmarks.Reporting.Configuration;
using IronXL;

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
            var benchNames = new string[] { "bench0", "bench1", "bench2", "bench3", "bench4", "bench5", "bench6", "bench7", "bench8", "bench9", "bench10", };
            var benchmarksData = new List<BenchmarkData>()
            {
                CreateBenchmarkData(ReportDataType.MeanTime, contenders, benchNames),
                CreateBenchmarkData(ReportDataType.MemoryAlloc, contenders, benchNames)
            };

            // Act
            var reportName = reportGenerator.GenerateReport(benchmarksData, "ExcelLibsTests");

            // Assert
            var workbook = WorkBook.Load(reportName);

            Assert.Equal(benchmarksData.Count, workbook.WorkSheets.Count);
            Assert.Equal("Performance", workbook.WorkSheets[0].Name);
            Assert.Equal("Memory Allocation", workbook.WorkSheets[1].Name);

            foreach (var sheet in workbook.WorkSheets)
            {
                Assert.Equal(benchNames.Length, sheet.Charts.Count);
                AssertDataPlacedCorrectly(sheet, reportConfig, contenders, benchNames);
                AssertChartsPlacedCorrectly(sheet, reportConfig, contenders.Length);
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
                    entry.Add(benchNames[j], j + 1);
                }

                data.DataEntries.Add(entry);
            }

            return data;
        }

        private static void AssertDataPlacedCorrectly(WorkSheet sheet, ReportingConfig reportConfig, string[] contenders, string[] benchNames)
        {
            for (var i = 0; i < contenders.Length; i++)
            {
                Assert.Equal(contenders[i], sheet[$"A{reportConfig.DataTableStartingRow + i + 1}"].Value);
            }

            for (var i = 0; i < benchNames.Length; i++)
            {
                Assert.Equal(benchNames[i], sheet[$"{letters[i + 2]}{reportConfig.DataTableStartingRow}"].Value);
            }

            for (var i = 0; i < contenders.Length; i++)
            {
                for (var j = 0; j < benchNames.Length; j++)
                {
                    Assert.Equal(j + 1.0, sheet[$"{letters[j + 2]}{reportConfig.DataTableStartingRow + i + 1}"].Value);
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