namespace IronBenchmarks.Reporting.Configuration
{
    public interface IReportingConfig
    {
        string ReportsFolder { get; set; }
        int ChartWidth { get; set; }
        int ChartHeight { get; set; }
        int DataTableStartingRow { get; set; }
        int ChartsInRow { get; set; }
    }
}