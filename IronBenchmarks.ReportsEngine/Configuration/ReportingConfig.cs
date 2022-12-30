namespace IronBenchmarks.Reporting.Configuration
{
    public class ReportingConfig : IReportingConfig
    {
        public string ReportsFolder { get; set; } = "Reports";
        public int ChartWidth { get; set; } = 6;
        public int ChartHeight { get; set; } = 18;
        public int DataTableStartingRow { get; set; } = 1;
        public int ChartsInRow { get; set; } = 4;
        public bool AppendToLastReport { get; set; } = false;
    }
}