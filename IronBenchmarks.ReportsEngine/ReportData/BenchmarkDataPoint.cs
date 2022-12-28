namespace IronBenchmarks.Reporting.ReportData
{
    public class BenchmarkDataPoint
    {
        public string Name { get; }
        public Units UnitType { get; }
        public double Value { get; }

        public BenchmarkDataPoint(string name, Units unitType, double value)
        {
            Name = name;
            UnitType = unitType;
            Value = value;
        }
    }
}
