using BenchmarkDotNet.Attributes;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class FormulaCellBenchmark : BenchmarkBase
    {
        private readonly string formula = "=A1/B1";

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            AsposeCells[$"A2"].Formula = formula;
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").FormulaA1 = formula;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Formula = formula;
        }

        [Benchmark]
        public override void IronXl()
        {
            IxlSheet[$"A2"].Formula = formula;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            IxlOldSheet[$"A2"].Formula = formula;
        }

        [Benchmark]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(1).CreateCell(0).SetCellFormula(formula.Replace("=", ""));
        }
    }
}
