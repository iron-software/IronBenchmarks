using System.Collections.Generic;
using System.Linq;

namespace IronBenchmarks.Reporting.ReportData
{
    public class BenchmarkDataEntry
    {
        private readonly Dictionary<string, double> data = new Dictionary<string, double>();

        public string Name { get; }
        public int Count => data.Count;

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
}
