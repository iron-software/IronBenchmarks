using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class InsertRowBenchmark : SheetOperationsBenchmarkBase
    {
        [Benchmark]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            AsposeCells.InsertRow(0);
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            _ = ClosedXmlSheet.Row(1).InsertRowsBelow(1);
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            EpplusSheet.InsertRow(1, 1);
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            _ = IronXlSheet.InsertRow(0);
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet.InsertRow(0);
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            NpoiSheet.ShiftRows(0, NpoiSheet.LastRowNum, 1);
            _ = NpoiSheet.CreateRow(0);
        }
    }
}
