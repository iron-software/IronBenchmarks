using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    public class AccessingRangePropertiesBenchmark : SheetOperationsBenchmarkBase
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
            _ = IronXlSheet["A1:CV100"].Style.Font;
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            _ = Iron_XlOldSheet["A1:CV100"].Style.Font;
        }

        [Benchmark]
        public override void Npoi()
        {

        }
    }
}
