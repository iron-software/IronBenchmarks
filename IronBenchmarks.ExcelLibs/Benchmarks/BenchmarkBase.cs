using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using Microsoft.Extensions.Configuration;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System.Reflection;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    public abstract class BenchmarkBase
    {
        public readonly Cells AsposeCells;
        public readonly IXLWorksheet ClosedXmlSheet;
        public readonly ExcelPackage EpplusExcelPackage;
        public readonly ExcelWorksheet EpplusSheet;
        public readonly IronXLOld.WorkSheet IxlOldSheet;
        public readonly IronXL.WorkSheet IxlSheet;
        public readonly ISheet NpoiSheet;

        public BenchmarkBase()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var configuration = builder.Build();
            var appConfig = configuration.GetSection("AppConfig");
            var licenseKeyIronXl = appConfig["LicenseKeyIronXl"];

            IronXL.License.LicenseKey = licenseKeyIronXl;
            IronXLOld.License.LicenseKey = licenseKeyIronXl;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            IxlSheet = new IronXL.WorkBook().DefaultWorkSheet;

            IxlOldSheet = new IronXLOld.WorkBook().DefaultWorkSheet;

            AsposeCells = new Workbook().Worksheets[0].Cells;

            NpoiSheet = new XSSFWorkbook().CreateSheet();

            ClosedXmlSheet = new XLWorkbook().Worksheets.Add("Sheet1");

            EpplusExcelPackage = new ExcelPackage();
            EpplusSheet = EpplusExcelPackage.Workbook.Worksheets.Add("Sheet1");
        }

        [Benchmark]
        public abstract void IronXl();

        [Benchmark]
        public abstract void IronXlOld();

        [Benchmark(Baseline = true)]
        public abstract void Aspose();

        [Benchmark]
        public abstract void Npoi();

        [Benchmark]
        public abstract void CloseXml();

        [Benchmark]
        public abstract void Epplus();

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            IxlSheet.WorkBook.Close();
            IxlOldSheet.WorkBook.Close();
            AsposeCells.Dispose();
            NpoiSheet.Workbook.Close();
            ClosedXmlSheet.Workbook.Dispose();
            EpplusExcelPackage.Dispose();
        }
    }
}