using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class FormulaCellBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly string _formula = "=A1/B1";

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            AsposeCells[$"A2"].Formula = _formula;
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").FormulaA1 = _formula;
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Formula = _formula;
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            IronXlSheet[$"A2"].Formula = _formula;
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet[$"A2"].Formula = _formula;
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(1).CreateCell(0).SetCellFormula(_formula.Replace("=", ""));
        }
    }
}
