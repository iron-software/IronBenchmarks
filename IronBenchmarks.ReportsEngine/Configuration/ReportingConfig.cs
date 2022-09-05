namespace IronBenchmarks.Reporting.Configuration
{
    public class ReportingConfig : IReportingConfig
    {
        public string ReportsFolder { get; set; } = "Reports";
        public int ChartWidth { get; set; } = 11;
        public int ChartHeight { get; set; } = 24;
        public int TimeTableStartingRow { get; set; } = 27;
        public string ChartTitle { get; set; }
    }
}