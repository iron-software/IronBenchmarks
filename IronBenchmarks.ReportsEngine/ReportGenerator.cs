﻿using BenchmarkDotNet.Reports;
using IronBenchmarks.Reporting.Configuration;
using IronXL;
using IronXL.Drawing.Charts;
using IronXL.Formatting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using static BenchmarkDotNet.Reports.SummaryTable;

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

        public void GenerateReport(List<Summary> summaries, string reportTag)
        {
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            var timeTable = GetDataFromSummary(summaries, ReportDataType.MeanTime);
            var memoryTable = GetDataFromSummary(summaries, ReportDataType.MemoryAlloc);

            GenerateReport(timeTable, $"{reportTag}$Performance");
            GenerateReport(memoryTable, $"{reportTag}$Memory Allocation");
        }

        public string GenerateReport(Dictionary<string, Dictionary<string, double>> chartsData, string reportTag)
        {
            EnsureReportsFolderExists();

            var report = LoadTemplate();

            FillReport(report, chartsData, reportTag.Split('$')[1]);

            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var reportName = Path.Combine(path ?? "", $"{_reportConfig.ReportsFolder}\\{reportTag.Replace("$", " ")}_Report_{DateTime.Now:yyyy-MM-d_HH-mm-ss}.xlsx");

            report.SaveAs(reportName);

            return reportName;
        }

        private void FillReport(WorkBook report, Dictionary<string, Dictionary<string, double>> timeTableData, string chartTitle)
        {
            var sheet = report.DefaultWorkSheet;

            _numberOfConteders = timeTableData.Count;
            _numberOfBenchmarks = timeTableData.FirstOrDefault().Value.Values.Count;
            _headerRowAddress = $"B{_reportConfig.DataTableStartingRow}:{_letters[_numberOfBenchmarks + 1]}{_reportConfig.DataTableStartingRow}";

            ClearTemplateMockData(sheet);

            var benchmarkTitles = timeTableData.Values.FirstOrDefault()?.Keys.ToArray();

            FillHeader(sheet, _headerRowAddress, benchmarkTitles);

            var i = 1;

            foreach (var contender in timeTableData)
            {
                FillRow(sheet, i, contender, benchmarkTitles);

                i++;
            }

            UpdateCharts(sheet, chartTitle);
        }

        private void ClearTemplateMockData(WorkSheet sheet)
        {
            FillHeader(sheet, "B27:K27", null);

            ClearSeriesRow(sheet, 1);
        }

        private WorkBook CreateTemplate()
        {
            var template = WorkBook.Create(ExcelFileFormat.XLSX);
            var sheet = template.DefaultWorkSheet;

            PutInMockData(sheet);

            AddChart(sheet, 1, 10, "Performance");

            FormatTimeTable(sheet, 1, 10);

            return template.SaveAs("template.xlsx");
        }

        private WorkBook LoadTemplate()
        {
            if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\template.xlsx"))
            {
                return WorkBook.Load("template.xlsx");
            }

            return CreateTemplate();
        }

        private void EnsureReportsFolderExists()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var reportsFolder = Path.Combine(path ?? "", _reportConfig.ReportsFolder);

            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
        }

        private static void FillHeader(WorkSheet sheet, string headerRowAddress, string[] benchmarkList)
        {
            benchmarkList = benchmarkList ?? new string[] { "" };

            var i = 0;

            foreach (var cell in sheet[headerRowAddress])
            {
                var benchmarkTitle = i >= benchmarkList.Length ? "" : benchmarkList[i];

                if (benchmarkTitle.Length > 0)
                {
                    int lastDotIndex = benchmarkTitle.LastIndexOf(".");
                    int firstDashIndex = benchmarkTitle.IndexOf("-");

                    benchmarkTitle = benchmarkTitle.Substring(lastDotIndex + 1, firstDashIndex - lastDotIndex - 1);
                }

                cell.Value = benchmarkTitle;

                i++;
            }
        }

        private void FillRow(WorkSheet sheet, int i, KeyValuePair<string, Dictionary<string, double>>? contender, string[] benchmarkTitles)
        {
            var seriesRowNumber = _reportConfig.DataTableStartingRow + i;
            var seriesRowAddress = $"B{seriesRowNumber}:{_letters[_numberOfBenchmarks + 1]}{seriesRowNumber}";
            var times = new double[benchmarkTitles.Length];

            for (var j = 0; j < benchmarkTitles.Length; j++)
            {
                times[j] = (double)(contender?.Value[benchmarkTitles[j]]);
            }

            PutInSeriesData(sheet, seriesRowAddress, times);

            sheet[$"A{seriesRowNumber}"].Value = contender?.Key;
        }

        private void ClearSeriesRow(WorkSheet sheet, int i)
        {
            var seriesRowNumber = _reportConfig.DataTableStartingRow + i;
            var seriesRowAddress = $"B{seriesRowNumber}:K{seriesRowNumber}";

            foreach (var cell in sheet[seriesRowAddress])
            {
                cell.Value = "";
            }

            sheet[$"A{seriesRowNumber}"].Value = "";
        }

        private void FormatTimeTable(WorkSheet sheet, int numberOfRowsToFormat, int numberOfColumnsToFormat)
        {
            for (var i = 1; i <= numberOfRowsToFormat; i++)
            {
                var seriesRowNumber = _reportConfig.DataTableStartingRow + i;
                var seriesRowAddress = $"B{seriesRowNumber}:{_letters[numberOfColumnsToFormat + 1]}{seriesRowNumber}";

                FormatRow(sheet, seriesRowAddress);
            }
        }

        private void PutInMockData(WorkSheet sheet)
        {
            PutInMockHeaderData(sheet, $"B{_reportConfig.DataTableStartingRow}:K{_reportConfig.DataTableStartingRow}");

            PutInMockTimeTableData(sheet);
        }

        private void PutInMockTimeTableData(WorkSheet sheet)
        {
            for (var i = 1; i <= 1; i++)
            {
                var seriesRowNumber = _reportConfig.DataTableStartingRow + i;
                var seriesRowAddress = $"B{seriesRowNumber}:K{seriesRowNumber}";

                PutInMockSeriesData(sheet, seriesRowAddress);

                sheet[$"A{seriesRowNumber}"].Value = $"Contender_{seriesRowNumber}";
            }
        }

        private static void PutInMockSeriesData(WorkSheet sheet, string seriesRowAddress)
        {
            var rnd = new Random();
            var times = new double[10];

            for (var i = 0; i < times.Length; i++)
            {
                times[i] = rnd.Next(25, 100);
            }

            PutInSeriesData(sheet, seriesRowAddress, times);
        }

        private static void PutInSeriesData(WorkSheet sheet, string seriesRowAddress, double[] times)
        {
            var i = 0;

            foreach (var cell in sheet[seriesRowAddress])
            {
                cell.Value = i >= times.Length ? 0 : times[i];

                i++;
            }
        }

        private static void FormatRow(WorkSheet sheet, string rowAddress)
        {
            sheet[rowAddress].FormatString = BuiltinFormats.Number0;
        }

        private static void AutoSizeIimeTable(WorkSheet sheet, string headerAddress)
        {
            foreach (var cell in sheet[headerAddress])
            {
                sheet.AutoSizeColumn(cell.ColumnIndex);
            }
        }

        private static void PutInMockHeaderData(WorkSheet sheet, string headerRowAddress)
        {
            foreach (var cell in sheet[headerRowAddress])
            {
                cell.Value = $"Mock_Benchmark_{cell.ColumnIndex}";
            }
        }

        private void AddChart(WorkSheet sheet, int numberOfSeriesToAdd, int numberOfBenchmarks, string chartTitle)
        {
            for(int i = 1; i <= numberOfBenchmarks; i++)
            {
                var chart = sheet.CreateChart(ChartType.Bar, 0, (i - 1) * _reportConfig.ChartWidth, _reportConfig.ChartHeight, i * _reportConfig.ChartWidth);
                var headerAddress = $"{_letters[i + 1]}{_reportConfig.DataTableStartingRow}:{_letters[i + 1]}{_reportConfig.DataTableStartingRow}";

                for (var j = 1; j <= numberOfSeriesToAdd; j++)
                {
                    var seriesRowNumber = _reportConfig.DataTableStartingRow + j;
                    var seriesRowAddress = $"{_letters[i + 1]}{seriesRowNumber}:{_letters[i + 1]}{seriesRowNumber}";

                    var range = sheet[seriesRowAddress];
                    range.FormatString = BuiltinFormats.Number0;

                    var series = chart.AddSeries(seriesRowAddress, headerAddress);
                    series.Title = sheet[$"A{seriesRowNumber}"].StringValue;
                }

                var benchmarkName = sheet[headerAddress].StringValue;

                chart.SetTitle($"{chartTitle}\n{benchmarkName}");
                chart.SetLegendPosition(LegendPosition.Bottom);
                chart.Plot();
            }
        }

        private static void RemoveCharts(WorkSheet sheet)
        {
            foreach (var chart in sheet.Charts.ToArray())
            {
                sheet.RemoveChart(chart);
            }
        }

        private void UpdateCharts(WorkSheet sheet, string chartTitle)
        {
            RemoveCharts(sheet);

            AddChart(sheet, _numberOfConteders, _numberOfBenchmarks, chartTitle);

            FormatTimeTable(sheet, _numberOfConteders, _numberOfBenchmarks);

            AutoSizeIimeTable(sheet, _headerRowAddress);
        }

        private Dictionary<string, Dictionary<string, double>> GetDataFromSummary(List<Summary> summaries, ReportDataType dataType)
        {
            var timeTable = new Dictionary<string, Dictionary<string, double>>();

            foreach (var summary in summaries)
            {
                SummaryTableColumn resultsColumn;

                switch (dataType)
                {
                    case ReportDataType.MemoryAlloc:
                        resultsColumn = summary.Table.Columns.FirstOrDefault(c => c.Header == "Allocated");
                        break;
                    case ReportDataType.MeanTime:
                        resultsColumn = summary.Table.Columns.FirstOrDefault(c => c.Header == "Mean");
                        break;
                    default:
                        return timeTable;
                }

                var i = 0;

                foreach (var benchmarkCase in summary.BenchmarksCases)
                {
                    var valueString = resultsColumn.Content[i];
                    valueString = valueString.Substring(0, valueString.IndexOf(" "));

                    var value = double.Parse(valueString);
                    var methodName = benchmarkCase.Descriptor.WorkloadMethod.Name;

                    if (!timeTable.ContainsKey(methodName))
                    {
                        timeTable.Add(methodName, new Dictionary<string, double>());
                    }

                    var times = timeTable[methodName];
                    times.Add(summary.Title, value);

                    i++;
                }
            }

            return timeTable;
        }
    }

    internal enum ReportDataType
    {
        MeanTime,
        MemoryAlloc
    }
}