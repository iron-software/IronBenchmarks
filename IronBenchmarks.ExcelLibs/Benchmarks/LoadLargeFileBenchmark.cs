using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class LoadLargeFileBenchmark
    {
        private readonly string largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";

        public LoadLargeFileBenchmark()
        {
            BenchmarkBase.SetupLicenses();
        }

        [Benchmark(Baseline = true)]
        public void Aspose()
        {
            _ = new Workbook(largeFileName);
        }

        [Benchmark]
        public void ClosedXml()
        {
            _ = new XLWorkbook(largeFileName);
        }

        [Benchmark]
        public void Epplus()
        {
            _ = new ExcelPackage(largeFileName);
        }

        [Benchmark]
        public void IronXl()
        {
            _ = IronXL.WorkBook.Load(largeFileName);
        }

        [Benchmark]
        public void IronXlOld()
        {
            _ = IronXLOld.WorkBook.Load(largeFileName);
        }

        [Benchmark]
        public void Npoi()
        {
            _ = new XSSFWorkbook(largeFileName);
        }
    }
}
