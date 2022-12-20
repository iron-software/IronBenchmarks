namespace IronBenchmarks.Reporting.Configuration
{
    public class ReportingConfig : IReportingConfig
    {
        public string ReportsFolder { get; set; } = "Reports";
        public int ChartWidth { get; set; } = 11;
        public int ChartHeight { get; set; } = 24;
        public int DataTableStartingRow { get; set; } = 27;
    }
}