using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks.Bases
{
    public abstract class SheetOperationsBenchmarkBase : BenchmarkBase
    {
        private readonly Random _rand = new Random();

        public Cells AsposeCells;
        public IXLWorksheet ClosedXmlSheet;
        public ExcelPackage EpplusExcelPackage;
        public ExcelWorksheet EpplusSheet;
        public IronXLOld.WorkSheet Iron_XlOldSheet;
        public IronXL.WorkSheet IronXlSheet;
        public ISheet NpoiSheet;

        [IterationSetup]
        public void IterationSetup()
        {
            int cell1Val = _rand.Next();
            int cell2Val = _rand.Next();

            IronXlSheet = new IronXL.WorkBook().DefaultWorkSheet;
            IronXlSheet["A1"].Value = cell1Val;
            IronXlSheet["B1"].Value = cell2Val;

            Iron_XlOldSheet = new IronXLOld.WorkBook().DefaultWorkSheet;
            Iron_XlOldSheet["A1"].Value = cell1Val;
            Iron_XlOldSheet["B1"].Value = cell2Val;

            AsposeCells = new Workbook().Worksheets[0].Cells;
            AsposeCells["A1"].PutValue(cell1Val);
            AsposeCells["B1"].PutValue(cell2Val);

            NpoiSheet = new XSSFWorkbook().CreateSheet();
            NpoiSheet.CreateRow(0).CreateCell(0).SetCellValue(cell1Val);
            NpoiSheet.GetRow(0).CreateCell(1).SetCellValue(cell2Val);

            ClosedXmlSheet = new XLWorkbook().Worksheets.Add("Sheet1");
            ClosedXmlSheet.Cell("A1").Value = cell1Val;
            ClosedXmlSheet.Cell("B1").Value = cell2Val;

            EpplusExcelPackage = new ExcelPackage();
            EpplusSheet = EpplusExcelPackage.Workbook.Worksheets.Add("Sheet1");
            EpplusSheet.Cells["A1"].Value = cell1Val;
            EpplusSheet.Cells["B1"].Value = cell2Val;
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            IronXlSheet.WorkBook.Close();
            IronXlSheet = null;
            Iron_XlOldSheet.WorkBook.Close();
            Iron_XlOldSheet = null;
            AsposeCells.Dispose();
            AsposeCells = null;
            NpoiSheet.Workbook.Close();
            NpoiSheet = null;
            ClosedXmlSheet.Workbook.Dispose();
            ClosedXmlSheet = null;
            EpplusExcelPackage.Dispose();
            EpplusExcelPackage = null;
            EpplusSheet.Dispose();
            EpplusSheet = null;
        }
    }
}