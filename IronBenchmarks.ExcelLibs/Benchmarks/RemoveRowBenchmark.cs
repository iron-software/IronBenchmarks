using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using IronBenchmarks.ExcelLibs.Config;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    {
        private readonly string _removeRowFileName = "RemoveRowFiles\\RemoveRow.xlsx";
        private IronXL.WorkSheet _ixlSheet;
        private IronXLOld.WorkSheet _ixlOldSheet;
        private Workbook _asposeWb;
        private Cells _asposeSheet;
        private IXLWorksheet _closedXmlSheet;
        private XSSFSheet _npoiSheet;
        private XSSFRow _npoiRow;
        private ExcelPackage _epplusWb;
        private ExcelWorksheet _epplusSheet;

        [GlobalSetup]
        public void IterationSetup()
        {
            _asposeWb = new Workbook(_removeRowFileName);
            _asposeSheet = _asposeWb.Worksheets[0].Cells;

            _ixlSheet = IronXL.WorkBook.Load(_removeRowFileName).DefaultWorkSheet;

            _ixlOldSheet = IronXLOld.WorkBook.Load(_removeRowFileName).DefaultWorkSheet;

            _npoiSheet = (XSSFSheet)new XSSFWorkbook(_removeRowFileName).GetSheetAt(0);
            _npoiRow = (XSSFRow)_npoiSheet.GetRow(1);

            _closedXmlSheet = new XLWorkbook(_removeRowFileName).Worksheet("Remove");

            _epplusWb = new ExcelPackage(_removeRowFileName);
            _epplusSheet = _epplusWb.Workbook.Worksheets[0];
        }

        [GlobalCleanup]
        public void IterationCleanup()
        {
            _ixlSheet.WorkBook.Close();
            _ixlSheet = null;

            _ixlOldSheet.WorkBook.Close();
            _ixlOldSheet = null;

            _asposeWb.Dispose();
            _asposeSheet.Dispose();
            _asposeWb = null;
            _asposeSheet = null;

            _closedXmlSheet.Worksheet.Workbook.Dispose();
            _closedXmlSheet = null;

            _epplusSheet.Dispose();
            _epplusSheet = null;
            _epplusWb.Dispose();
            _epplusWb = null;

        }

        [Benchmark]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            _ = _asposeSheet.DeleteRows(0, 1, true);
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            _closedXmlSheet.Rows(1, 1).Delete();
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            _epplusSheet.DeleteRow(1);
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            _ixlSheet.RemoveRow(1);
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            _ixlOldSheet.Rows[1].RemoveRow();
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            _npoiSheet.RemoveRow(_npoiRow);
        }
    }
}
