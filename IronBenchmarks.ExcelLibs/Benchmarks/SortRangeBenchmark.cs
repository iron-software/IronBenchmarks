using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SortRangeBenchmark
    {
        private readonly string sortRangeFileName = "SortRangeFiles\\SortRange.xlsx";
        private IronXL.WorkSheet ixlSortRange;
        private IronXLOld.WorkSheet ixlOldSortRange;
        private Workbook asposeSortRangeWb;
        private DataSorter asposeSorter;
        private Cells asposeCells;
        private readonly CellArea asposeCellArea = new CellArea
        {
            StartRow = 0,
            StartColumn = 0,
            EndRow = 999,
            EndColumn = 100
        };
        private IXLRange closedXmlSortRange;
        private XSSFSheet npoiSortRange;
        private ExcelPackage epplusSortRangeWb;
        private ExcelWorksheet epplusSortRangeSheet;
        private ExcelRange epplusSortRange;

        public SortRangeBenchmark()
        {
            BenchmarkBase.SetupLicenses();
            BenchmarkBase.EnsureResultsFolderExists();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            asposeSortRangeWb = new Workbook(sortRangeFileName);
            asposeCells = asposeSortRangeWb.Worksheets[0].Cells;
            asposeSorter = GetAsposeSorter();

            ixlSortRange = IronXL.WorkBook.Load(sortRangeFileName).DefaultWorkSheet;

            ixlOldSortRange = IronXLOld.WorkBook.Load(sortRangeFileName).DefaultWorkSheet;

            npoiSortRange = (XSSFSheet)new XSSFWorkbook(sortRangeFileName).GetSheetAt(0);

            closedXmlSortRange = new XLWorkbook(sortRangeFileName).Worksheet("ToSort").Range("A1:CV1000");

            epplusSortRangeWb = new ExcelPackage(sortRangeFileName);
            epplusSortRangeSheet = epplusSortRangeWb.Workbook.Worksheets[0];
            epplusSortRange = epplusSortRangeSheet.Cells["A1:CV1000"];
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            ixlSortRange.WorkBook.Close();
            ixlSortRange = null;

            ixlOldSortRange.WorkBook.Close();
            ixlOldSortRange = null;

            asposeSortRangeWb.Dispose();
            asposeCells.Dispose();
            asposeSortRangeWb = null;
            asposeCells = null;
            asposeSorter = null;

            closedXmlSortRange.Worksheet.Workbook.Dispose();
            closedXmlSortRange = null;

            epplusSortRange.Dispose();
            epplusSortRange = null;
            epplusSortRangeSheet.Dispose();
            epplusSortRangeSheet = null;
            epplusSortRangeWb.Dispose();
            epplusSortRangeWb = null;

        }

        [Benchmark(Baseline = true)]
        public void Aspose()
        {
            asposeSorter.Sort(asposeCells, asposeCellArea);
        }

        [Benchmark]
        public void ClosedXml()
        {
            closedXmlSortRange.Sort(1);
        }

        [Benchmark]
        public void Epplus()
        {
            epplusSortRange.Sort(x => x.SortBy.Column(0));
        }

        [Benchmark]
        public void IronXl()
        {
            ixlSortRange.SortByColumn(0, IronXL.SortOrder.Ascending);
        }

        [Benchmark]
        public void IronXlOld()
        {
            ixlOldSortRange.SortByColumn(0, IronXLOld.SortOrder.Ascending);
        }

        [Benchmark]
        public void Npoi()
        {

        }

        private DataSorter GetAsposeSorter()
        {
            var sorter = asposeSortRangeWb.DataSorter;
            sorter.Order1 = SortOrder.Ascending;
            sorter.Key1 = 0;

            return sorter;
        }
    }
}
