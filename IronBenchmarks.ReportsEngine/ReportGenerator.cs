using BenchmarkDotNet.Reports;
using IronBenchmarks.Reporting.Configuration;
using IronBenchmarks.Reporting.ReportData;
using IronXL;
using IronXL.Drawing.Charts;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace IronBenchmarks.Reporting
{
    public class ReportGenerator
    {
        private readonly IReportingConfig _reportConfig;
        private string _headerRowAddress = "B27:K27";
        private int _numberOfConteders = 0;
        private int _numberOfBenchmarks = 0;
        private static readonly Dictionary<int, string> _letters = new Dictionary<int, string>()
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

        public ReportGenerator(IReportingConfig reportConfig)
        {
            _reportConfig = reportConfig;
        }

        public string GenerateReport(List<Summary> summaries, string reportTag, Dictionary<string, string> contedersNamesAndVersions = null)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var benchmarksData = new List<BenchmarkData>()
            {
                new BenchmarkData(summaries, ReportDataType.MeanTime),
                new BenchmarkData(summaries, ReportDataType.MemoryAlloc)
            };

            return GenerateReport(benchmarksData, reportTag, contedersNamesAndVersions);
        }

        public string GenerateReport(List<BenchmarkData> chartsData, string reportTag, Dictionary<string, string> contedersNamesAndVersions = null)
        {
            contedersNamesAndVersions = contedersNamesAndVersions ?? new Dictionary<string, string>();

            EnsureReportsFolderExists();

            WorkBook report = _reportConfig.AppendToLastReport ? GetLastReport() : WorkBook.Create();

            FillReport(report, chartsData, contedersNamesAndVersions);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string reportName = Path.Combine(path ?? "", $"{_reportConfig.ReportsFolder}\\{reportTag}_Report_{DateTime.Now:yyyy-MM-d_HH-mm-ss}.xlsx");

            _ = report.SaveAs(reportName);

            return reportName;
        }

        private WorkBook GetLastReport()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string reportsFolder = Path.Combine(path ?? "", $"{_reportConfig.ReportsFolder}");
            string[] files = Directory.GetFiles(reportsFolder);
            IOrderedEnumerable<string> sortedFiles = files.OrderByDescending(f => File.GetLastWriteTime(f));
            string mostRecentFile = sortedFiles.First();

            return WorkBook.Load(mostRecentFile);
        }

        private void FillReport(WorkBook report, List<BenchmarkData> tablesData, Dictionary<string, string> contedersNamesAndVersions)
        {
            foreach (BenchmarkData chartData in tablesData)
            {
                WorkSheet sheet = _reportConfig.AppendToLastReport
                    ? report.GetWorkSheet($"{EnumHelper.GetEnumDescription(chartData.DataType)}")
                    : report.CreateWorkSheet($"{EnumHelper.GetEnumDescription(chartData.DataType)}");

                if (_reportConfig.AppendToLastReport)
                {
                    _numberOfBenchmarks = chartData.GetNumberOfBenchmarks() < GetLastColumnInReport(sheet)
                        ? GetLastColumnInReport(sheet)
                        : chartData.GetNumberOfBenchmarks();
                }
                else
                {
                    _numberOfBenchmarks = chartData.GetNumberOfBenchmarks();
                }

                _numberOfConteders = chartData.GetNumberOfContenders() + (_reportConfig.AppendToLastReport ? GetLastRowInReport(sheet) - 1 : 0);
                _headerRowAddress = $"B{_reportConfig.DataTableStartingRow}:{_letters[_numberOfBenchmarks + 1]}{_reportConfig.DataTableStartingRow}";

                Dictionary<string, Units> benchmarkTitles = chartData.GetBenchmarkNames();

                if (!_reportConfig.AppendToLastReport)
                {
                    FillHeader(sheet, _headerRowAddress, benchmarkTitles);
                }

                int i = _reportConfig.AppendToLastReport ? GetLastRowInReport(sheet) : 1;

                foreach (BenchmarkDataEntry contender in chartData.DataEntries)
                {
                    FillRow(sheet, i, contender, benchmarkTitles, contedersNamesAndVersions);

                    i++;
                }

                UpdateCharts(sheet, EnumHelper.GetEnumDescription(chartData.DataType));
            }
        }

        private int GetLastColumnInReport(WorkSheet sheet)
        {
            RangeColumn[] allColums = sheet.AllColumnsInRange;
            RangeColumn lastColumn = allColums[allColums.Length - 1];

            return lastColumn.RangeAddress.LastColumn;
        }

        private int GetLastRowInReport(WorkSheet sheet)
        {
            RangeRow[] allRows = sheet.AllRowsInRange;
            RangeRow lastRow = allRows[allRows.Length - 1];

            return lastRow.RangeAddress.LastRow;
        }

        private void EnsureReportsFolderExists()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string reportsFolder = Path.Combine(path ?? "", _reportConfig.ReportsFolder);

            if (!Directory.Exists(reportsFolder))
            {
                _ = Directory.CreateDirectory(reportsFolder);
            }
        }

        private static void FillHeader(WorkSheet sheet, string headerRowAddress, Dictionary<string, Units> benchmarkList)
        {
            benchmarkList = benchmarkList ?? new Dictionary<string, Units>();

            int i = 0;
            Cell[] cells = sheet[headerRowAddress].ToArray();

            foreach (KeyValuePair<string, Units> benchmark in benchmarkList)
            {
                cells[i].Value = $"{GetBenchmarkTitle(benchmark.Key)}, {EnumHelper.GetEnumDescription(benchmark.Value)}";

                i++;
            }
        }

        private static string GetBenchmarkTitle(string benchmarkTitle)
        {
            if (benchmarkTitle.Length > 0)
            {
                int lastDotIndex = benchmarkTitle.LastIndexOf(".");
                int firstDashIndex = benchmarkTitle.IndexOf("-");

                benchmarkTitle = lastDotIndex < 0 || firstDashIndex < 0
                    ? benchmarkTitle
                    : benchmarkTitle.Substring(lastDotIndex + 1, firstDashIndex - lastDotIndex - 1);
            }

            return benchmarkTitle;
        }

        private void FillRow(WorkSheet sheet, int i, BenchmarkDataEntry contender, Dictionary<string, Units> benchmarkTitles, Dictionary<string, string> contedersNamesAndVersions)
        {
            int seriesRowNumber = _reportConfig.DataTableStartingRow + i;
            string seriesRowAddress = $"B{seriesRowNumber}:{_letters[_numberOfBenchmarks + 1]}{seriesRowNumber}";
            double[] times = new double[benchmarkTitles.Count];
            int j = 0;

            foreach (KeyValuePair<string, Units> benchmark in benchmarkTitles)
            {
                times[j] = contender[benchmark.Key].Value;

                j++;
            }

            PutInSeriesData(sheet, seriesRowAddress, times);

            string contederTitle = GetContenderTitle(contender, contedersNamesAndVersions);

            sheet[$"A{seriesRowNumber}"].Value = contederTitle;
        }

        private string GetContenderTitle(BenchmarkDataEntry contender, Dictionary<string, string> contedersNamesAndVersions)
        {
            return contedersNamesAndVersions == null || !contedersNamesAndVersions.ContainsKey(contender.Name)
                ? contender.Name
                : $"{contender.Name}, v.{contedersNamesAndVersions[contender.Name]}";
        }

        private void FormatTimeTable(WorkSheet sheet, int numberOfRowsToFormat, int numberOfColumnsToFormat)
        {
            int seriesRowNumber = _reportConfig.DataTableStartingRow + 1;
            string seriesRowAddress = $"B{seriesRowNumber}:{_letters[numberOfColumnsToFormat + 1]}{seriesRowNumber + numberOfRowsToFormat}";
            sheet[seriesRowAddress].FormatString = BuiltinFormats.Thousands2;
        }

        private static void PutInSeriesData(WorkSheet sheet, string seriesRowAddress, double[] times)
        {
            int i = 0;

            foreach (Cell cell in sheet[seriesRowAddress])
            {
                cell.Value = i >= times.Length ? 0 : times[i];

                i++;
            }
        }

        private static void FormatHeader(WorkSheet sheet, string headerAddress)
        {
            foreach (Cell cell in sheet[headerAddress])
            {
                cell.Style.WrapText = true;
            }
        }

        private void AddCharts(WorkSheet sheet, int numberOfSeriesToAdd, int numberOfBenchmarks, string chartTitle)
        {
            int chartRow = 0;
            int chartsInRow = 0;

            for (int i = 1; i <= numberOfBenchmarks; i++)
            {
                IChart chart = CreateChart(sheet, numberOfSeriesToAdd, chartRow, chartsInRow);
                string headerAddress = $"{_letters[i + 1]}{_reportConfig.DataTableStartingRow}:{_letters[i + 1]}{_reportConfig.DataTableStartingRow}";

                for (int j = 1; j <= numberOfSeriesToAdd; j++)
                {
                    int seriesRowNumber = _reportConfig.DataTableStartingRow + j;
                    string seriesRowAddress = $"{_letters[i + 1]}{seriesRowNumber}:{_letters[i + 1]}{seriesRowNumber}";

                    if (!sheet[seriesRowAddress].First().IsNumeric)
                    {
                        continue;
                    }

                    IChartSeries series = chart.AddSeries(seriesRowAddress, headerAddress);
                    series.Title = sheet[$"A{seriesRowNumber}"].StringValue;
                }

                string benchmarkName = sheet[headerAddress].StringValue;

                chart.SetTitle($"{chartTitle}\n{benchmarkName}");
                chart.SetLegendPosition(LegendPosition.Bottom);
                chart.Plot();

                chartsInRow++;

                if (chartsInRow >= _reportConfig.ChartsInRow)
                {
                    chartRow++;
                    chartsInRow = 0;
                }
            }
        }

        private IChart CreateChart(WorkSheet sheet, int chartsStartingRow, int chartRow, int chartsInRow)
        {
            int topRow = chartsStartingRow + 1 + (_reportConfig.ChartHeight * chartRow);
            int bottomRow = topRow + _reportConfig.ChartHeight;
            int firstColumn = _reportConfig.ChartWidth * chartsInRow;
            int lastColumn = firstColumn + _reportConfig.ChartWidth;
            IChart chart = sheet.CreateChart(ChartType.Bar, topRow, firstColumn, bottomRow, lastColumn);
            return chart;
        }

        private static void RemoveCharts(WorkSheet sheet)
        {
            foreach (IChart chart in sheet.Charts.ToArray())
            {
                sheet.RemoveChart(chart);
            }
        }

        private void UpdateCharts(WorkSheet sheet, string chartTitle)
        {
            RemoveCharts(sheet);

            AddCharts(sheet, _numberOfConteders, _numberOfBenchmarks, chartTitle);

            FormatTimeTable(sheet, _numberOfConteders, _numberOfBenchmarks);

            FormatHeader(sheet, _headerRowAddress);
        }
    }
}
