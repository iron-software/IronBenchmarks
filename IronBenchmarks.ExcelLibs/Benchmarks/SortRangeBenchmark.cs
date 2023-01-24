using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SortRangeBenchmark : BenchmarkBase
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
        public override void Aspose()
        {
            _ = asposeSorter.Sort(asposeCells, asposeCellArea);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            _ = closedXmlSortRange.Sort(1);
        }

        [Benchmark]
        public override void Epplus()
        {
            epplusSortRange.Sort(x => x.SortBy.Column(0));
        }

        [Benchmark]
        public override void IronXl()
        {
            _ = ixlSortRange.SortByColumn(0, IronXL.SortOrder.Ascending);
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            _ = ixlOldSortRange.SortByColumn(0, IronXLOld.SortOrder.Ascending);
        }

        [Benchmark]
        public override void Npoi()
        {
            throw new NotImplementedException();
        }

        private DataSorter GetAsposeSorter()
        {
            DataSorter sorter = asposeSortRangeWb.DataSorter;
            sorter.Order1 = SortOrder.Ascending;
            sorter.Key1 = 0;

            return sorter;
        }
    }
}
