using BenchmarkDotNet.Attributes;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class FormulaCellBenchmark : BenchmarkBase
    {
        private readonly Random rand = new Random();
        private readonly string formula = "=A1/B1";

        public FormulaCellBenchmark() : base()
        {
            var cell1Val = rand.Next();
            var cell2Val = rand.Next();

            IxlSheet["A1"].Value = cell1Val;
            IxlSheet["B1"].Value = cell2Val;

            IxlOldSheet["A1"].Value = cell1Val;
            IxlOldSheet["B1"].Value = cell2Val;

            AsposeCells["A1"].PutValue(cell1Val);
            AsposeCells["B1"].PutValue(cell2Val);

            NpoiSheet.CreateRow(0).CreateCell(0).SetCellValue(cell1Val);
            NpoiSheet.GetRow(0).CreateCell(1).SetCellValue(cell2Val);

            ClosedXmlSheet.Cell("A1").Value = cell1Val;
            ClosedXmlSheet.Cell("B1").Value = cell2Val;

            EpplusSheet.Cells["A1"].Value = cell1Val;
            EpplusSheet.Cells["B1"].Value = cell2Val;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            AsposeCells[$"A{rand.Next(2, 1000000)}"].Formula = formula;
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A{rand.Next(2, 1000000)}").FormulaA1 = formula;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A{rand.Next(2, 1000000)}"].Formula = formula;
        }

        [Benchmark]
        public override void IronXl()
        {
            IxlSheet[$"A{rand.Next(2, 1000000)}"].Formula = formula;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            IxlOldSheet[$"A{rand.Next(2, 1000000)}"].Formula = formula;
        }

        [Benchmark]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(rand.Next(2, 1000000)).CreateCell(0).SetCellFormula(formula.Replace("=", ""));
        }
    }
}
