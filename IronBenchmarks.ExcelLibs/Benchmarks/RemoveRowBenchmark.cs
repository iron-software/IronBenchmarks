using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class RemoveRowBenchmark : SheetOperationsBenchmarkBase
    {
        [Benchmark]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            _ = AsposeCells.DeleteRows(0, 1, true);
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Rows(1, 1).Delete();
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            EpplusSheet.DeleteRow(1);
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            IronXlSheet.RemoveRow(0);
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet.Rows[0].RemoveRow();
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            NpoiSheet.RemoveRow(NpoiSheet.GetRow(0));
        }
    }
}
