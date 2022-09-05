namespace IronBenchmarks.Reporting.Configuration
{
    public interface IReportingConfig
    {
        string ReportsFolder { get; set; }
        int ChartWidth { get; set; }
        int ChartHeight { get; set; }
        int TimeTableStartingRow { get; set; }
        string ChartTitle { get; set; }
    }
}