using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using NPOI.SS.UserModel;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DateCellBenchmark : BenchmarkBase
    {
        private readonly DateTime date = DateTime.Now;
        private readonly Random rand = new Random();
        private ICellStyle npoiStyle = null;

        public DateCellBenchmark() : base()
        {

        }

        [Benchmark]
        public override void IronXl()
        {
            IxlSheet[$"A{rand.Next(1, 1000000)}"].Value = date;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            IxlOldSheet[$"A{rand.Next(1, 1000000)}"].Value = date;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            var style = new CellsFactory().CreateStyle();
            style.Number = 15;

            var cell = AsposeCells[$"A{rand.Next(1, 1000000)}"];
            cell.PutValue(date);
            cell.SetStyle(style);
        }

        [Benchmark]
        public override void Npoi()
        {
            npoiStyle = npoiStyle ?? NpoiSheet.Workbook.CreateCellStyle();
            npoiStyle.DataFormat = NpoiSheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

            var row = NpoiSheet.CreateRow(rand.Next(1, 1000000));

            var cell = row.CreateCell(0);
            cell.SetCellValue(date);
            cell.CellStyle = npoiStyle;
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A{rand.Next(1, 1000000)}").Value = date;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A{rand.Next(1, 1000000)}"].Value = date;
            EpplusSheet.Cells[$"A{rand.Next(1, 1000000)}"].Style.Numberformat.Format = "mm/dd/yyyy";
        }
    }
}
