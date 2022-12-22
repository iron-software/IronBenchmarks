using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using OfficeOpenXml.Style;
using System;
using System.Linq;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class StyleChangeBenchmark : BenchmarkBase
    {
        private readonly Random rand = new Random();
        private ICellStyle npoiStyle = null;
        private IFont npoiFont = null;
        private readonly string cellValue = "Cell";

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            var style = new CellsFactory().CreateStyle();
            style.Font.Size = 22;
            style.VerticalAlignment = TextAlignmentType.Top;
            style.HorizontalAlignment = TextAlignmentType.Right;

            var cell = AsposeCells[$"A{rand.Next(1, 1000000)}"];
            cell.PutValue(cellValue);
            cell.SetStyle(style);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            var cell = ClosedXmlSheet.Cell($"A{rand.Next(1, 1000000)}");
            cell.Value = cellValue;

            var style = cell.Style;

            style.Font.FontSize = 22;
            style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }

        [Benchmark]
        public override void Epplus()
        {
            var cell = EpplusSheet.Cells[$"A{rand.Next(1, 1000000)}"].FirstOrDefault();
            cell.Value = cellValue;

            cell.Style.Font.Size = 22;
            cell.Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }

        [Benchmark]
        public override void IronXl()
        {
            var range = IxlSheet[$"A{rand.Next(1, 1000000)}"];
            range.Value = cellValue;

            var style = range.Style;

            style.Font.Height = 22;
            style.VerticalAlignment = IronXL.Styles.VerticalAlignment.Top;
            style.HorizontalAlignment = IronXL.Styles.HorizontalAlignment.Right;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            var range = IxlOldSheet[$"A{rand.Next(1, 1000000)}"];
            range.Value = cellValue;

            var style = range.Style;

            style.Font.Height = 22;
            style.VerticalAlignment = IronXLOld.Styles.VerticalAlignment.Top;
            style.HorizontalAlignment = IronXLOld.Styles.HorizontalAlignment.Right;
        }

        [Benchmark]
        public override void Npoi()
        {
            npoiFont = npoiFont ?? NpoiSheet.Workbook.CreateFont();
            npoiFont.FontHeightInPoints = 22;

            npoiStyle = npoiStyle ?? NpoiSheet.Workbook.CreateCellStyle();
            npoiStyle.SetFont(npoiFont);
            npoiStyle.VerticalAlignment = VerticalAlignment.Top;
            npoiStyle.Alignment = HorizontalAlignment.Right;

            var row = NpoiSheet.CreateRow(rand.Next(1, 1000000));
            var cell = row.CreateCell(0);
            cell.SetCellValue(cellValue);
            cell.CellStyle = npoiStyle;
        }
    }
}
