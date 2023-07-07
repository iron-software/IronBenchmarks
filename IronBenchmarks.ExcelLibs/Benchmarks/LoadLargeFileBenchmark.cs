using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class LoadLargeFileBenchmark : BenchmarkBase
    {
        private readonly string _largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            _ = new Workbook(_largeFileName);
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            _ = new XLWorkbook(_largeFileName);
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            _ = new ExcelPackage(_largeFileName);
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            _ = IronXL.WorkBook.Load(_largeFileName);
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            _ = IronXLOld.WorkBook.Load(_largeFileName);
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            _ = new XSSFWorkbook(_largeFileName);
        }
    }
}
