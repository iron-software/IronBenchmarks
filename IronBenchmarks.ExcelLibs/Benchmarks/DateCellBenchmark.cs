using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Collections.Generic;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class DateCellBenchmark
    {
        private readonly IronXL.WorkSheet ixlSheet;
        private readonly IronXLOld.WorkSheet ixlOldSheet;
        private readonly Cells asposeCells;
        private readonly ISheet npoiSheet = null;
        private readonly IXLWorksheet closedXmlSheet;
        private readonly ExcelPackage epplusExcelPackage;
        private readonly ExcelWorksheet epplusSheet;
        private readonly DateTime date = DateTime.Now;
        private readonly Random rand = new Random();
        private ICellStyle npoiStyle;

        public DateCellBenchmark(string ironXlKey)
        {
            IronXL.License.LicenseKey = ironXlKey;
            IronXLOld.License.LicenseKey = ironXlKey;

            ixlSheet = new IronXL.WorkBook().DefaultWorkSheet;

            ixlOldSheet = new IronXLOld.WorkBook().DefaultWorkSheet;

            asposeCells = new Workbook().Worksheets[0].Cells;

            npoiSheet = new XSSFWorkbook().CreateSheet();

            closedXmlSheet = new XLWorkbook().Worksheets.Add("Sheet1");

            epplusExcelPackage = new ExcelPackage();
            epplusSheet = epplusExcelPackage.Workbook.Worksheets.Add("Sheet1");
        }

        [Benchmark]
        public void IronXlDateCell()
        {
            ixlSheet[$"A{rand.Next(1, 1000000)}"].Value = date;
        }

        [Benchmark]
        public void IronXlOldDateCell()
        {
            ixlOldSheet[$"A{rand.Next(1, 1000000)}"].Value = date;
        }

        [Benchmark(Baseline = true)]
        public void AsposeDateCell()
        {
            var style = new CellsFactory().CreateStyle();
            style.Number = 15;

            var cell = asposeCells[$"A{rand.Next(1, 1000000)}"];
            cell.PutValue(date);
            cell.SetStyle(style);
        }

        [Benchmark]
        public void NpoiDateCell()
        {
            npoiStyle = npoiSheet is null ? npoiSheet.Workbook.CreateCellStyle() : npoiStyle;
            npoiStyle.DataFormat = npoiSheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

            var row = npoiSheet.CreateRow(rand.Next(1, 1000000));

            var cell = row.CreateCell(0);
            cell.SetCellValue(date);
            cell.CellStyle = npoiStyle;
        }

        [Benchmark]
        public void CloseXmlDateCell()
        {
            closedXmlSheet.Cell($"A{rand.Next(1, 1000000)}").Value = date;
        }

        [Benchmark]
        public void EpplusDateCell()
        {
            epplusSheet.Cells[$"A{rand.Next(1, 1000000)}"].Value = date;
            epplusSheet.Cells[$"A{rand.Next(1, 1000000)}"].Style.Numberformat.Format = "mm/dd/yyyy";
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ixlSheet.WorkBook.Close();
            ixlOldSheet.WorkBook.Close();
            asposeCells.Dispose();
            npoiSheet.Workbook.Close();
            closedXmlSheet.Workbook.Dispose();
            epplusExcelPackage.Dispose();
        }
    }
}
