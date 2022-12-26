using Aspose.Cells;
using BenchmarkDotNet.Attributes;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    public class AccessingRangePropertiesBenchmark : BenchmarkBase
    {
        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            var range = AsposeCells.CreateRange("A1", "CV100");

            foreach (Cell cell in range)
            {
                _ = cell.GetStyle().Font;
            }
        }

        [Benchmark]
        public override void ClosedXml()
        {
            _ = ClosedXmlSheet.Range("A1:CV1000").Style.Font;
        }

        [Benchmark]
        public override void Epplus()
        {
            _ = EpplusSheet.Cells["A1:CV1000"].Style.Font;
        }

        [Benchmark]
        public override void IronXl()
        {
            _ = IxlSheet["A1:CV100"].Style.Font;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            _ = IxlOldSheet["A1:CV100"].Style.Font;
        }

        [Benchmark]
        public override void Npoi()
        {

        }
    }
}
