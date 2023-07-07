using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class FormulaCellBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly string _formula = "=A1/B1";

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            AsposeCells[$"A2"].Formula = _formula;
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").FormulaA1 = _formula;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Formula = _formula;
        }

        [Benchmark]
        public override void IronXl()
        {
            IronXlSheet[$"A2"].Formula = _formula;
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet[$"A2"].Formula = _formula;
        }

        [Benchmark]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(1).CreateCell(0).SetCellFormula(_formula.Replace("=", ""));
        }
    }
}
