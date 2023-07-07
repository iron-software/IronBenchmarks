using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System.IO;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class SaveLargeFileBenchmark : BenchmarkBase
    {
        private readonly string _largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";
        private IronXL.WorkBook _ixlLargeFile;
        private IronXLOld.WorkBook _ixlOldLargeFile;
        private Workbook _asposeLargeFile;
        private XLWorkbook _closedXmlLargeFile;
        private XSSFWorkbook _npoiLargeFile;
        private ExcelPackage _epplusLargeFile;

        [GlobalSetup]
        public void Setup()
        {
            _ixlLargeFile = IronXL.WorkBook.Load(_largeFileName);
            _ixlOldLargeFile = IronXLOld.WorkBook.Load(_largeFileName);
            _asposeLargeFile = new Workbook(_largeFileName);
            _closedXmlLargeFile = new XLWorkbook(_largeFileName);
            _npoiLargeFile = new XSSFWorkbook(_largeFileName);
            _epplusLargeFile = new ExcelPackage(_largeFileName);
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            _asposeLargeFile.Save("Results\\Aspose_large.xlsx");
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            _closedXmlLargeFile.SaveAs("Results\\ClosedXML_large.xlsx");
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            _epplusLargeFile.SaveAs("Results\\Epplus_large.xlsx");
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            _ = _ixlLargeFile.SaveAs("Results\\IronXL_large.xlsx");
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            _ = _ixlOldLargeFile.SaveAs("Results\\IronXLOld_large.xlsx");
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            _npoiLargeFile.Write(File.Create("Results\\Npoi_large.xlsx"));
        }
    }
}
