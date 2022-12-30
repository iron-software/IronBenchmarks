using System;
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

namespace IronBenchmarks
{
    public static class EnumHelper
    {
        public static string GetEnumDescription(Enum value)
        {
            var field = value.GetType().GetField(value.ToString());
            return !(Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
                ? value.ToString()
                : attribute.Description;
        }
    }
}
