using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Filters;

namespace IronBenchmarks.ExcelLibs.Config
{
    public class ExcelConfig : ManualConfig
    {
        public ExcelConfig()
        {
            _ = AddFilter(new NameFilter(name => name.Contains("IronXl")));
        }
    }
}
