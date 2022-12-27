using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DateCellBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly DateTime date = DateTime.Now;

        [Benchmark]
        public override void IronXl()
        {
            IxlSheet[$"A2"].Value = date;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            IxlOldSheet[$"A2"].Value = date;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            var style = new CellsFactory().CreateStyle();
            style.Number = 15;

            var cell = AsposeCells[$"A1"];
            cell.PutValue(date);
            cell.SetStyle(style);
        }

        [Benchmark]
        public override void Npoi()
        {
            var npoiStyle = NpoiSheet.Workbook.CreateCellStyle();
            npoiStyle.DataFormat = NpoiSheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

            var row = NpoiSheet.CreateRow(1);

            var cell = row.CreateCell(0);
            cell.SetCellValue(date);
            cell.CellStyle = npoiStyle;
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").Value = date;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = date;
            EpplusSheet.Cells[$"A2"].Style.Numberformat.Format = "mm/dd/yyyy";
        }
    }
}
