using BenchmarkDotNet.Reports;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using static BenchmarkDotNet.Reports.SummaryTable;

namespace IronBenchmarks.Reporting
{
    public class BenchmarkData
    {
        public ReportDataType DataType { get; }

        public readonly List<BenchmarkDataEntry> DataEntries = new List<BenchmarkDataEntry>();

        public BenchmarkData(List<Summary> summaries, ReportDataType dataType)
        {
            DataType = dataType;

            foreach (var summary in summaries)
            {
                var resultsColumn = GetColumn(summary, dataType);
                var i = 0;

                foreach (var benchmarkCase in summary.BenchmarksCases)
                {
                    var value = GetValueFromColumn(resultsColumn, i);
                    var methodName = benchmarkCase.Descriptor.WorkloadMethod.Name;

                    var dataEntry = GetDataEntry(methodName);
                    dataEntry.Add(summary.Title, value);

                    i++;
                }
            }
        }

        public BenchmarkData(ReportDataType dataType)
        {
            DataType = dataType;
        }

        private SummaryTableColumn GetColumn(Summary summary, ReportDataType dataType)
        {
            switch (dataType)
            {
                case ReportDataType.MemoryAlloc:
                    return summary.Table.Columns.FirstOrDefault(c => c.Header == "Allocated");
                case ReportDataType.MeanTime:
                    return summary.Table.Columns.FirstOrDefault(c => c.Header == "Mean");
                default:
                    return null;
            }
        }

        private BenchmarkDataEntry GetDataEntry(string name)
        {
            if (!DataEntries.Any(e => e.Name == name))
            {
                AddDataEntry(name);
            }

            return DataEntries.FirstOrDefault(e => e.Name == name);
        }

        private void AddDataEntry(string name)
        {
            DataEntries.Add(new BenchmarkDataEntry(name));
        }

        private static double GetValueFromColumn(SummaryTableColumn resultsColumn, int i)
        {
            var valueString = resultsColumn.Content[i];
            valueString = valueString.IndexOf(" ") < 0
                ? valueString
                : valueString.Substring(0, valueString.IndexOf(" "));

            double.TryParse(valueString, out var value);
            return value;
        }

        public string[] GetBenchmarkNames()
        {
            return DataEntries.FirstOrDefault().GetKeys();
        }

        public int GetNumberOfContenders()
        {
            return DataEntries.Count();
        }

        public int GetNumberOfBenchmarks()
        {
            return DataEntries.FirstOrDefault().GetKeys().Length;
        }
    }

    public class BenchmarkDataEntry
    {
        private readonly Dictionary<string, double> data = new Dictionary<string, double>();

        public string Name { get; }

        public BenchmarkDataEntry(string name)
        {
            Name = name;
        }

        public void Add(string title, double value)
        {
            data.Add(title, value);
        }

        public string[] GetKeys()
        {
            return data.Keys.ToArray();
        }

        public double this[string key]
        {
            get
            {
                return data[key];
            }
        }
    }

    public enum ReportDataType
    {
        [Description("Performance")]
        MeanTime,
        [Description("Memory Allocation")]
        MemoryAlloc
    }
}
