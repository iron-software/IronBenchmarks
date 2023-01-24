using System.Collections.Generic;
using System.Linq;

namespace IronBenchmarks.Reporting.ReportData
{
    public class BenchmarkDataEntry
    {
        private readonly List<BenchmarkDataPoint> data = new List<BenchmarkDataPoint>();

        public string Name { get; }
        public int Count => data.Count;

        public BenchmarkDataEntry(string name)
        {
            Name = name;
        }

        public void Add(string title, Units units, double value)
        {
            data.Add(new BenchmarkDataPoint(title, units, value));
        }

        public Dictionary<string, Units> GetBenchmarkNames()
        {
            var names = new Dictionary<string, Units>();

            foreach (BenchmarkDataPoint point in data)
            {
                names.Add(point.Name, point.UnitType);
            }

            return names;
        }

        public BenchmarkDataPoint this[string key]
        {
            get
            {
                return data.FirstOrDefault(d => d.Name == key);
            }
        }
    }
}
