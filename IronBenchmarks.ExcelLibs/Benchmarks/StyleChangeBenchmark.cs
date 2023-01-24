using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using NPOI.SS.UserModel;
using OfficeOpenXml.Style;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class StyleChangeBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly string cellValue = "Cell";

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            Style style = new CellsFactory().CreateStyle();
            style.Font.Size = 22;
            style.VerticalAlignment = TextAlignmentType.Top;
            style.HorizontalAlignment = TextAlignmentType.Right;

            Cell cell = AsposeCells[$"A2"];
            cell.PutValue(cellValue);
            cell.SetStyle(style);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            IXLCell cell = ClosedXmlSheet.Cell($"A2");
            cell.Value = cellValue;

            IXLStyle style = cell.Style;

            style.Font.FontSize = 22;
            _ = style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            _ = style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = cellValue;

            EpplusSheet.Cells[$"A2"].Style.Font.Size = 22;
            EpplusSheet.Cells[$"A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            EpplusSheet.Cells[$"A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }

        [Benchmark]
        public override void IronXl()
        {
            IronXL.Range range = IronXlSheet[$"A2"];
            range.Value = cellValue;

            IronXL.Styles.IStyle style = range.Style;

            style.Font.Height = 22;
            style.VerticalAlignment = IronXL.Styles.VerticalAlignment.Top;
            style.HorizontalAlignment = IronXL.Styles.HorizontalAlignment.Right;
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            IronXLOld.Range range = Iron_XlOldSheet[$"A2"];
            range.Value = cellValue;

            IronXLOld.Styles.IStyle style = range.Style;

            style.Font.Height = 22;
            style.VerticalAlignment = IronXLOld.Styles.VerticalAlignment.Top;
            style.HorizontalAlignment = IronXLOld.Styles.HorizontalAlignment.Right;
        }

        [Benchmark]
        public override void Npoi()
        {
            IFont npoiFont = NpoiSheet.Workbook.CreateFont();
            npoiFont.FontHeightInPoints = 22;

            ICellStyle npoiStyle = NpoiSheet.Workbook.CreateCellStyle();
            npoiStyle.SetFont(npoiFont);
            npoiStyle.VerticalAlignment = VerticalAlignment.Top;
            npoiStyle.Alignment = HorizontalAlignment.Right;

            IRow row = NpoiSheet.CreateRow(1);
            ICell cell = row.CreateCell(0);
            cell.SetCellValue(cellValue);
            cell.CellStyle = npoiStyle;
        }
    }
}
