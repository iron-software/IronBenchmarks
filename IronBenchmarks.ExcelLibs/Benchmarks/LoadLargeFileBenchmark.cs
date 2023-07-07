using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class LoadLargeFileBenchmark : BenchmarkBase
    {
        private readonly string _largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            _ = new Workbook(_largeFileName);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            _ = new XLWorkbook(_largeFileName);
        }

        [Benchmark]
        public override void Epplus()
        {
            _ = new ExcelPackage(_largeFileName);
        }

        [Benchmark]
        public override void IronXl()
        {
            _ = IronXL.WorkBook.Load(_largeFileName);
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            _ = IronXLOld.WorkBook.Load(_largeFileName);
        }

        [Benchmark]
        public override void Npoi()
        {
            _ = new XSSFWorkbook(_largeFileName);
        }
    }
}
