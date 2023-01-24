using BenchmarkDotNet.Reports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static BenchmarkDotNet.Reports.SummaryTable;

namespace IronBenchmarks.Reporting.ReportData
{
    public class BenchmarkData
    {
        public ReportDataType DataType { get; }

        public readonly List<BenchmarkDataEntry> DataEntries = new List<BenchmarkDataEntry>();

        public BenchmarkData(List<Summary> summaries, ReportDataType dataType)
        {
            DataType = dataType;

            foreach (Summary summary in summaries)
            {
                SummaryTableColumn resultsColumn = GetColumn(summary, dataType);
                Units unitType = GetUnitType(resultsColumn);
                int i = 0;

                foreach (BenchmarkDotNet.Running.BenchmarkCase benchmarkCase in summary.BenchmarksCases)
                {
                    double value = GetValueFromColumn(resultsColumn, i);
                    string methodName = benchmarkCase.Descriptor.WorkloadMethod.Name;

                    BenchmarkDataEntry dataEntry = GetDataEntry(methodName);
                    dataEntry.Add(summary.Title, unitType, value);

                    i++;
                }
            }
        }

        public BenchmarkData(ReportDataType dataType)
        {
            DataType = dataType;
        }

        public Dictionary<string, Units> GetBenchmarkNames()
        {
            return DataEntries.FirstOrDefault().GetBenchmarkNames();
        }

        public int GetNumberOfContenders()
        {
            return DataEntries.Count();
        }

        public int GetNumberOfBenchmarks()
        {
            return DataEntries.FirstOrDefault().GetBenchmarkNames().Count;
        }

        private Units GetUnitType(SummaryTableColumn resultsColumn)
        {
            foreach (string dataPoint in resultsColumn.Content)
            {
                string unitString = dataPoint.IndexOf(" ") < 0 ? "None" : dataPoint.Substring(dataPoint.IndexOf(" ") + 1);
                unitString = Containsμ(unitString.Substring(0, 1)) ? "us" : unitString;

                if (Enum.TryParse(unitString, out Units units) && units != Units.None)
                {
                    return units;
                }
            }

            return Units.None;
        }

        private bool Containsμ(string text)
        {
            string μNormalized = "μ".ToString().Normalize(NormalizationForm.FormKD);
            string textNormalized = text.ToString().Normalize(NormalizationForm.FormKD);

            return μNormalized.Equals(textNormalized);
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
            string valueString = resultsColumn.Content[i];
            valueString = valueString.IndexOf(" ") < 0
                ? valueString
                : valueString.Substring(0, valueString.IndexOf(" "));

            _ = double.TryParse(valueString, out double value);
            return value;
        }
    }
}
