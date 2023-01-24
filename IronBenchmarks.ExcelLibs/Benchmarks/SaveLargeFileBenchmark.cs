using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System.IO;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SaveLargeFileBenchmark : BenchmarkBase
    {
        private readonly string largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";
        private IronXL.WorkBook ixlLargeFile;
        private IronXLOld.WorkBook ixlOldLargeFile;
        private Workbook asposeLargeFile;
        private XLWorkbook closedXmlLargeFile;
        private XSSFWorkbook npoiLargeFile;
        private ExcelPackage epplusLargeFile;

        [GlobalSetup]
        public void Setup()
        {
            ixlLargeFile = IronXL.WorkBook.Load(largeFileName);
            ixlOldLargeFile = IronXLOld.WorkBook.Load(largeFileName);
            asposeLargeFile = new Workbook(largeFileName);
            closedXmlLargeFile = new XLWorkbook(largeFileName);
            npoiLargeFile = new XSSFWorkbook(largeFileName);
            epplusLargeFile = new ExcelPackage(largeFileName);
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
            _ = ixlLargeFile.SaveAs("Results\\IronXL_large.xlsx");
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            _ = ixlOldLargeFile.SaveAs("Results\\IronXLOld_large.xlsx");
        }

        [Benchmark]
        public override void Npoi()
        {
            npoiLargeFile.Write(File.Create("Results\\Npoi_large.xlsx"));
        }
    }
}
