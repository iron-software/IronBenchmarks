namespace Benchmarks.ReportsEngine.Configuration
{
    public interface IAppConfig
    {
        string Environment { get; set; }
        string LicenseKeyIronXl { get; set; }
        string LicenseKeyIronPdf { get; set; }
        string LicenseKeyIronBarCode { get; set; }
        string ReportsFolder { get; set; }
        int ChartWidth { get; set; }
        int ChartHeight { get; set; }
        int TimeTableStartingRow { get; set; }

        string ChartTitle { get; set; }

        string ResultsFolderName { get; set; }
    }
}