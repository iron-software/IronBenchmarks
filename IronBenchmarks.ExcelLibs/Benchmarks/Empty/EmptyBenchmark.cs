using BenchmarkDotNet.Attributes;

namespace IronBenchmarks.ExcelLibs.Benchmarks.Empty
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class EmptyBenchmark
    {
        [Benchmark(Baseline = true)]
        public void Aspose()
        {
            _ = 1 + 1;
        }

        [Benchmark]
        public void ClosedXml()
        {
            _ = 1 + 1;
        }

        [Benchmark]
        public void Epplus()
        {
            _ = 1 + 1;
        }

        [Benchmark]
        public void IronXl()
        {
            _ = 1 + 1;
        }

        [Benchmark]
        public void IronXlOld()
        {
            _ = 1 + 1;
        }

        [Benchmark]
        public void Npoi()
        {
            _ = 1 + 1;
        }
    }
}
