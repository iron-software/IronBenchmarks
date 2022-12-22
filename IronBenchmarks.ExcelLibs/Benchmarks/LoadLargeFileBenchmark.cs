using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class LoadLargeFileBenchmark : BenchmarkBase
    {
        private readonly string largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            _ = new Workbook(largeFileName);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            _ = new XLWorkbook(largeFileName);
        }

        [Benchmark]
        public override void Epplus()
        {
            _ = new ExcelPackage(largeFileName);
        }

        [Benchmark]
        public override void IronXl()
        {
            _ = IronXL.WorkBook.Load(largeFileName);
        }

        [Benchmark]
        public override void IronXlOld()
        {
            _ = IronXLOld.WorkBook.Load(largeFileName);
        }

        [Benchmark]
        public override void Npoi()
        {
            _ = new XSSFWorkbook(largeFileName);
        }
    }
}
