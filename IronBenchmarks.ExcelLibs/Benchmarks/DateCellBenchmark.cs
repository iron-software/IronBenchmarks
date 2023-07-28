using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class DateCellBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly DateTime _date = DateTime.Now;

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            IronXlSheet[$"A2"].Value = _date;
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet[$"A2"].Value = _date;
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            Style style = new CellsFactory().CreateStyle();
            style.Number = 15;

            Cell cell = AsposeCells[$"A1"];
            cell.PutValue(_date);
            cell.SetStyle(style);
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            NPOI.SS.UserModel.ICellStyle npoiStyle = NpoiSheet.Workbook.CreateCellStyle();
            npoiStyle.DataFormat = NpoiSheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

            NPOI.SS.UserModel.IRow row = NpoiSheet.CreateRow(1);

            NPOI.SS.UserModel.ICell cell = row.CreateCell(0);
            cell.SetCellValue(_date);
            cell.CellStyle = npoiStyle;
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").Value = _date;
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = _date;
            EpplusSheet.Cells[$"A2"].Style.Numberformat.Format = "mm/dd/yyyy";
        }
    }
}
