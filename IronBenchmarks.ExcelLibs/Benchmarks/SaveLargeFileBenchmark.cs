using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SaveLargeFileBenchmark : BenchmarkBase
    {
        private readonly string largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";
        private readonly IronXL.WorkBook ixlLargeFile;
        private readonly IronXLOld.WorkBook ixlOldLargeFile;
        private readonly Workbook asposeLargeFile;
        private readonly XLWorkbook closedXmlLargeFile;
        private readonly XSSFWorkbook npoiLargeFile;
        private readonly ExcelPackage epplusLargeFile;

        public SaveLargeFileBenchmark() : base()
        {
            ixlLargeFile = IronXL.WorkBook.Load(largeFileName);
            ixlOldLargeFile = IronXLOld.WorkBook.Load(largeFileName);
            asposeLargeFile = new Workbook(largeFileName);
            closedXmlLargeFile = new XLWorkbook(largeFileName);
            npoiLargeFile = new XSSFWorkbook(largeFileName);
            epplusLargeFile = new ExcelPackage(largeFileName);

            EnsureResultsFolderExists();
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            asposeLargeFile.Save("Results\\Aspose_large.xlsx");
        }

        [Benchmark]
        public override void ClosedXml()
        {
            closedXmlLargeFile.SaveAs("Results\\ClosedXML_large.xlsx");
        }

        [Benchmark]
        public override void Epplus()
        {
            epplusLargeFile.SaveAs("Results\\Epplus_large.xlsx");
        }

        [Benchmark]
        public override void IronXl()
        {
            ixlLargeFile.SaveAs("Results\\IronXL_large.xlsx");
        }

        [Benchmark]
        public override void IronXlOld()
        {
            ixlOldLargeFile.SaveAs("Results\\IronXLOld_large.xlsx");
        }

        [Benchmark]
        public override void Npoi()
        {
            npoiLargeFile.Write(File.Create("Results\\IronXLOld_large.xlsx"));
        }

        private void EnsureResultsFolderExists()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var reportsFolder = Path.Combine(path ?? "", "Results");

            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
        }
    }
}
