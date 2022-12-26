using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.IO;
using System.Reflection;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    public abstract class BenchmarkBase
    {
        private readonly Random rand = new Random();

        public Cells AsposeCells;
        public IXLWorksheet ClosedXmlSheet;
        public ExcelPackage EpplusExcelPackage;
        public ExcelWorksheet EpplusSheet;
        public IronXLOld.WorkSheet IxlOldSheet;
        public IronXL.WorkSheet IxlSheet;
        public ISheet NpoiSheet;

        public BenchmarkBase()
        {
            SetupLicenses();
        }

        public static void SetupLicenses()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var configuration = builder.Build();
            var appConfig = configuration.GetSection("AppConfig");
            var licenseKeyIronXl = appConfig["LicenseKeyIronXl"];

            IronXL.License.LicenseKey = licenseKeyIronXl;
            IronXLOld.License.LicenseKey = licenseKeyIronXl;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static void EnsureResultsFolderExists()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var reportsFolder = Path.Combine(path ?? "", "Results");

            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            var cell1Val = rand.Next();
            var cell2Val = rand.Next();

            IxlSheet = new IronXL.WorkBook().DefaultWorkSheet;
            IxlSheet["A1"].Value = cell1Val;
            IxlSheet["B1"].Value = cell2Val;

            IxlOldSheet = new IronXLOld.WorkBook().DefaultWorkSheet;
            IxlOldSheet["A1"].Value = cell1Val;
            IxlOldSheet["B1"].Value = cell2Val;

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

        public abstract void IronXl();

        public abstract void IronXlOld();

        public abstract void Aspose();

        public abstract void Npoi();

        public abstract void ClosedXml();

        public abstract void Epplus();

        [IterationCleanup]
        public void IterationCleanup()
        {
            IxlSheet.WorkBook.Close();
            IxlSheet = null;
            IxlOldSheet.WorkBook.Close();
            IxlOldSheet = null;
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