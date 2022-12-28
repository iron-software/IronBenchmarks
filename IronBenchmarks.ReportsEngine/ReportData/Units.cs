using System.ComponentModel;

namespace IronBenchmarks.Reporting.ReportData
{
    public enum Units
    {
        [Description("None")]
        None = 0,
        [Description("Nanosecods")]
        ns,
        [Description("Microseconds")]
        us,
        [Description("Milliseconds")]
        ms,
        [Description("Seconds")]
        s,
        [Description("Minutes")]
        m,
        [Description("Hours")]
        h,
        [Description("Bits")]
        b,
        [Description("Bytes")]
        B,
        [Description("Kilobytes")]
        KB,
        [Description("Megabytes")]
        MB,
        [Description("Gigabytes")]
        GB
    }
}
