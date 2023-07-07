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
    public class InsertRowBenchmark : BenchmarkBase
    {
        private readonly string _insertRowFileName = "InsertRowFiles\\InsertRow.xlsx";
        private IronXL.WorkSheet _ixlSheet;
        private IronXLOld.WorkSheet _ixlOldSheet;
        private Workbook _asposeWb;
        private Cells _asposeSheet;
        private IXLWorksheet _closedXmlSheet;
        private XSSFSheet _npoiSheet;
        private XSSFRow _npoiRow;
        private ExcelPackage _epplusWb;
        private ExcelWorksheet _epplusSheet;

        [IterationSetup]
        public void IterationSetup()
        {
            _asposeWb = new Workbook(_insertRowFileName);
            _asposeSheet = _asposeWb.Worksheets[0].Cells;

            _ixlSheet = IronXL.WorkBook.Load(_insertRowFileName).DefaultWorkSheet;

            _ixlOldSheet = IronXLOld.WorkBook.Load(_insertRowFileName).DefaultWorkSheet;

            _npoiSheet = (XSSFSheet)new XSSFWorkbook(_insertRowFileName).GetSheetAt(0);
            _npoiRow = (XSSFRow)_npoiSheet.GetRow(1);

            _closedXmlSheet = new XLWorkbook(_insertRowFileName).Worksheet("Remove");

            _epplusWb = new ExcelPackage(_insertRowFileName);
            _epplusSheet = _epplusWb.Workbook.Worksheets[0];
        }

        [IterationCleanup]
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
        public override void Aspose()
        {
            _asposeSheet.InsertRow(1);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            _ = _closedXmlSheet.Row(1).InsertRowsBelow(1);
        }

        [Benchmark]
        public override void Epplus()
        {
            _epplusSheet.InsertRow(1, 1);
        }

        [Benchmark]
        public override void IronXl()
        {
            _ = _ixlSheet.InsertRow(1);
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            _ixlOldSheet.InsertRow(1);
        }

        [Benchmark]
        public override void Npoi()
        {
            _npoiSheet.ShiftRows(1, _npoiSheet.LastRowNum, 1);
            _ = _npoiSheet.CreateRow(1);
        }
    }
}
