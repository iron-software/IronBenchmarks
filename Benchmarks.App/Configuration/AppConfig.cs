namespace Benchmarks.Configuration;

public class AppConfig : IAppConfig
{
    public string? Environment { get; set; }
    public string? LicenseKeyIronXl { get; set; }
    public string? LicenseKeyIronBarCode { get; set; }
    public string ReportsFolder { get; set; } = "Reports";
    public int ChartWidth { get; set; } = 11;
    public int ChartHeight { get; set; } = 24;
    public int ContendersNumber { get; set; } = 2;
    public int TimeTableStartingRow { get; set; } = 27;

    public string? ChartTitle { get; set; }
    public string[]? BenchmarkList { get; set; }

    public string ResultsFolderName { get; set; } = "Results";
}