using Benchmarks.App.BenchmarkRunners;
using Benchmarks.Configuration;
using IronXL;
using IronXL.Drawing.Charts;
using IronXL.Formatting;
using System.Reflection;

namespace Benchmarks.Reporting;

public class ReportGenerator
{
    private readonly IAppConfig _appConfig;
    private readonly string headerRowAddress;

    public ReportGenerator(IAppConfig appConfig)
    {
        _appConfig = appConfig;
        headerRowAddress = $"B{_appConfig.TimeTableStartingRow}:K{_appConfig.TimeTableStartingRow}";
    }

    public string GenerateReport()
    {
        CreateReportsFolder();

        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var reportName = Path.Combine(path ?? "", $"{_appConfig.ReportsFolder}\\Report_{DateTime.Now:yyyy-MM-d_HH-mm-ss}.xlsx");
        var report = LoadTemplate();

        FillReport(report);

        report.SaveAs(reportName);

        return reportName;
    }

    private void FillReport(WorkBook report)
    {
        var timeTableData = RunBenchmarks();
        var sheet = report.DefaultWorkSheet;

        _appConfig.ContendersNumber = timeTableData.Count;

        FillHeader(sheet, headerRowAddress);

        var i = 1;

        foreach (var contender in timeTableData.Keys)
        {
            var times = timeTableData[contender];

            FillRow(sheet, i, contender, times);

            i++;
        }

        UpdateChart(sheet);
    }

    private Dictionary<string, TimeSpan[]> RunBenchmarks()
    {
        var curBarcodeRunner = new CurrentBarCodeBenchmarkRunner(_appConfig);
        var prevBarcodeRunner = new PreviousBarCodeBenchmarkRunner(_appConfig);

        return new()
        {
            { curBarcodeRunner.NameAndVersion, curBarcodeRunner.RunBenchmarks() },
            { prevBarcodeRunner.NameAndVersion, prevBarcodeRunner.RunBenchmarks() },
        };
    }

    private WorkBook CreateTemplate()
    {
        var template = WorkBook.Create(ExcelFileFormat.XLSX);
        var sheet = template.DefaultWorkSheet;

        PutInMockData(sheet);

        AddChart(sheet);

        FormatTimeTable(sheet);

        template.SaveAs("template.xlsx");
        template = WorkBook.Load("template.xlsx");

        return template;
    }

    private WorkBook LoadTemplate()
    {
        if (File.Exists(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\template.xlsx"))
        {
            return WorkBook.Load("template.xlsx");
        }

        return CreateTemplate();
    }

    private void CreateReportsFolder()
    {
        var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        var reportsFolder = Path.Combine(path ?? "", _appConfig.ReportsFolder);

        if (!Directory.Exists(reportsFolder))
        {
            Directory.CreateDirectory(reportsFolder);
        }
    }

    private void FillHeader(WorkSheet sheet, string headerRowAddress)
    {
        var benchmarkList = _appConfig.BenchmarkList ?? new string[] { "couldn't get benchmark list from config" };

        var i = 0;

        foreach (var cell in sheet[headerRowAddress])
        {
            cell.Value = i >= benchmarkList.Length ? "" : benchmarkList[i];

            i++;
        }
    }

    private void FillRow(WorkSheet sheet, int i, string contender, TimeSpan[] times)
    {
        var seriesRowNumber = _appConfig.TimeTableStartingRow + i;
        var seriesRowAddress = $"B{seriesRowNumber}:K{seriesRowNumber}";

        PutInSeriesData(sheet, seriesRowAddress, times);

        sheet[$"A{seriesRowNumber}"].Value = contender;
    }

    private void FormatTimeTable(WorkSheet sheet)
    {
        for (var i = 1; i <= _appConfig.ContendersNumber; i++)
        {
            var seriesRowNumber = _appConfig.TimeTableStartingRow + i;
            var seriesRowAddress = $"B{seriesRowNumber}:K{seriesRowNumber}";

            FormatRow(sheet, seriesRowAddress);
        }
    }

    private void PutInMockData(WorkSheet sheet)
    {
        PutInMockHeaderData(sheet, headerRowAddress);

        PutInMockTimeTableData(sheet);
    }

    private void PutInMockTimeTableData(WorkSheet sheet)
    {
        for (var i = 1; i <= _appConfig.ContendersNumber; i++)
        {
            var seriesRowNumber = _appConfig.TimeTableStartingRow + i;
            var seriesRowAddress = $"B{seriesRowNumber}:K{seriesRowNumber}";

            PutInMockSeriesData(sheet, seriesRowAddress);

            sheet[$"A{seriesRowNumber}"].Value = $"Contender_{seriesRowNumber}";
        }
    }

    private static void PutInMockSeriesData(WorkSheet sheet, string seriesRowAddress)
    {
        var rnd = new Random();
        var times = new TimeSpan[10];

        for (var i = 0; i < times.Length; i++)
        {
            times[i] = TimeSpan.FromSeconds(rnd.Next(25, 100));
        }

        PutInSeriesData(sheet, seriesRowAddress, times);
    }

    private static void PutInSeriesData(WorkSheet sheet, string seriesRowAddress, TimeSpan[] times)
    {
        var secondsInADay = 60 * 60 * 24;

        var i = 0;

        foreach (var cell in sheet[seriesRowAddress])
        {
            cell.Value = i >= times.Length ? 0 : times[i].TotalSeconds / secondsInADay;

            i++;
        }
    }

    private static void FormatRow(WorkSheet sheet, string rowAddress)
    {
        sheet[rowAddress].FormatString = BuiltinFormats.Duration3;
    }

    private static void PutInMockHeaderData(WorkSheet sheet, string headerRowAddress)
    {
        foreach (var cell in sheet[headerRowAddress])
        {
            cell.Value = $"Mock_Benchmark_{cell.ColumnIndex}";
        }
    }

    private void AddChart(WorkSheet sheet)
    {
        var chart = sheet.CreateChart(ChartType.Bar, 0, 0, _appConfig.ChartHeight, _appConfig.ChartWidth);

        for (var i = 1; i <= _appConfig.ContendersNumber; i++)
        {
            var seriesRowNumber = _appConfig.TimeTableStartingRow + i;
            var seriesRowAddress = $"B{seriesRowNumber}:K{seriesRowNumber}";

            var range = sheet[seriesRowAddress];
            range.FormatString = BuiltinFormats.Number0;

            var series = chart.AddSeries(seriesRowAddress, headerRowAddress);
            series.Title = sheet[$"A{seriesRowNumber}"].StringValue;
        }

        chart.SetTitle(_appConfig.ChartTitle);
        chart.SetLegendPosition(LegendPosition.Bottom);
        chart.Plot();
    }

    private static void RemoveChart(WorkSheet sheet)
    {
        var chart = sheet.Charts.FirstOrDefault();

        if (chart != null)
        {
            sheet.Charts.Remove(chart);
        }
    }

    private void UpdateChart(WorkSheet sheet)
    {
        RemoveChart(sheet);

        AddChart(sheet);

        FormatTimeTable(sheet);
    }
}