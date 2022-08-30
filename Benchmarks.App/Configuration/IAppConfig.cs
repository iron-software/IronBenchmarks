namespace Benchmarks.Configuration;

public interface IAppConfig
{
    public string? Environment { get; set; }
    public string? LicenseKeyIronXl { get; set; }
    public string? LicenseKeyIronBarCode { get; set; }
    public string ReportsFolder { get; set; }
    public int ChartWidth { get; set; }
    public int ChartHeight { get; set; }
    public int ContendersNumber { get; set; }
    public int TimeTableStartingRow { get; set; }

    public string? ChartTitle { get; set; }
    public string[]? BenchmarkList { get; set; }

    public string ResultsFolderName { get; set; }
}