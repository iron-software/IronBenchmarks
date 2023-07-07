using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;
using NPOI.SS.UserModel;
using OfficeOpenXml.Style;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class StyleChangeBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly string _cellValue = "Cell";

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            Style style = new CellsFactory().CreateStyle();
            style.Font.Size = 22;
            style.VerticalAlignment = TextAlignmentType.Top;
            style.HorizontalAlignment = TextAlignmentType.Right;

            Cell cell = AsposeCells[$"A2"];
            cell.PutValue(_cellValue);
            cell.SetStyle(style);
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            IXLCell cell = ClosedXmlSheet.Cell($"A2");
            cell.Value = _cellValue;

            IXLStyle style = cell.Style;

            style.Font.FontSize = 22;
            _ = style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            _ = style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = _cellValue;

            EpplusSheet.Cells[$"A2"].Style.Font.Size = 22;
            EpplusSheet.Cells[$"A2"].Style.VerticalAlignment = ExcelVerticalAlignment.Top;
            EpplusSheet.Cells[$"A2"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            IronXL.Range range = IronXlSheet[$"A2"];
            range.Value = _cellValue;

            IronXL.Styles.IStyle style = range.Style;

            style.Font.Height = 22;
            style.VerticalAlignment = IronXL.Styles.VerticalAlignment.Top;
            style.HorizontalAlignment = IronXL.Styles.HorizontalAlignment.Right;
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            IronXLOld.Range range = Iron_XlOldSheet[$"A2"];
            range.Value = _cellValue;

            IronXLOld.Styles.IStyle style = range.Style;

            style.Font.Height = 22;
            style.VerticalAlignment = IronXLOld.Styles.VerticalAlignment.Top;
            style.HorizontalAlignment = IronXLOld.Styles.HorizontalAlignment.Right;
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
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
            cell.SetCellValue(_cellValue);
            cell.CellStyle = npoiStyle;
        }
    }
}
