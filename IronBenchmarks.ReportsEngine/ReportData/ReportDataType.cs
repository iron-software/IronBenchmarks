using System.ComponentModel;

namespace IronBenchmarks.Reporting.ReportData
{
    public enum ReportDataType
    {
        [Description("Performance")]
        MeanTime,
        [Description("Memory Allocation")]
        MemoryAlloc
    }
}
