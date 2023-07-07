using System.Collections.Generic;
using System.Linq;

namespace IronBenchmarks.Reporting.ReportData
{
    public class BenchmarkDataEntry
    {
        private readonly List<BenchmarkDataPoint> _data = new List<BenchmarkDataPoint>();

        public string Name { get; }
        public int Count => _data.Count;

        public BenchmarkDataEntry(string name)
        {
            Name = name;
        }

        public void Add(string title, Units units, double value)
        {
            _data.Add(new BenchmarkDataPoint(title, units, value));
        }

        public Dictionary<string, Units> GetBenchmarkNames()
        {
            var names = new Dictionary<string, Units>();

            foreach (BenchmarkDataPoint point in _data)
            {
                names.Add(point.Name, point.UnitType);
            }

            return names;
        }

        public BenchmarkDataPoint this[string key]
        {
            get
            {
                return _data.FirstOrDefault(d => d.Name == key);
            }
        }
    }
}
