using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using ClosedXML.Excel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using System;
using System.Globalization;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    //[ShortRunJob]
    [MemoryDiagnoser]
    public class RandomCellsBenchmark
    {
        private readonly IronXL.WorkSheet ixlSheet;
        private readonly IronXLOld.WorkSheet ixlOldSheet;
        private readonly Cells asposeCells;
        private readonly ISheet npoiSheet;
        private readonly IXLWorksheet closedXmlSheet;
        private readonly ExcelPackage epplusExcelPackage;
        private readonly ExcelWorksheet epplusSheet;
        private readonly Random rand = new Random();
        private readonly string guidAsFormula = $"=\"{Guid.NewGuid()}\"";
        private readonly string guid = Guid.NewGuid().ToString();
        private readonly string date;
        private readonly decimal deci;
        private readonly int rnd32;
        private readonly int rnd13;

        public RandomCellsBenchmark(string ironXlKey)
        {
            IronXL.License.LicenseKey = ironXlKey;
            IronXLOld.License.LicenseKey = ironXlKey;

            date = GetRandomDate(rand);
            deci = GetRandomDecimal(rand);
            rnd32 = rand.Next(32);
            rnd13 = rand.Next(13);

            ixlSheet = new IronXL.WorkBook().DefaultWorkSheet;

            ixlOldSheet = new IronXLOld.WorkBook().DefaultWorkSheet;

            asposeCells = new Workbook().Worksheets[0].Cells;

            npoiSheet = new XSSFWorkbook().CreateSheet();

            closedXmlSheet = new XLWorkbook().Worksheets.Add("Sheet1");

            epplusExcelPackage = new ExcelPackage();
            epplusSheet = epplusExcelPackage.Workbook.Worksheets.Add("Sheet1");
        }

        [Benchmark]
        public void IronXlRandomCells()
        {
            var rowNum = rand.Next(1, 1000000);

            ixlSheet[$"A{rowNum}"].Value = guidAsFormula;
            ixlSheet[$"B{rowNum}"].Value = guid;
            ixlSheet[$"C{rowNum}"].Value = rnd32;
            ixlSheet[$"D{rowNum}"].Value = rnd13;
            ixlSheet[$"E{rowNum}"].Value = date;
            ixlSheet[$"F{rowNum}"].Value = deci;
        }

        [Benchmark]
        public void IronXlOldRandomCells()
        {
            var rowNum = rand.Next(1, 1000000);

            ixlOldSheet[$"A{rowNum}"].Value = guidAsFormula;
            ixlOldSheet[$"B{rowNum}"].Value = guid;
            ixlOldSheet[$"C{rowNum}"].Value = rnd32;
            ixlOldSheet[$"D{rowNum}"].Value = rnd13;
            ixlOldSheet[$"E{rowNum}"].Value = date;
            ixlOldSheet[$"F{rowNum}"].Value = deci;
        }

        [Benchmark(Baseline = true)]
        public void AsposeRandomCells()
        {
            var rowNum = rand.Next(1, 1000000);

            asposeCells[$"A{rowNum}"].Value = guidAsFormula;
            asposeCells[$"B{rowNum}"].Value = guid;
            asposeCells[$"C{rowNum}"].Value = rnd32;
            asposeCells[$"D{rowNum}"].Value = rnd13;
            asposeCells[$"E{rowNum}"].Value = date;
            asposeCells[$"F{rowNum}"].Value = deci;
        }

        [Benchmark]
        public void NpoiRandomCells()
        {
            var rowNum = rand.Next(0, 1000000);

            var row = npoiSheet.CreateRow(rowNum);

            row.CreateCell(0).SetCellValue(guidAsFormula);
            row.CreateCell(0).SetCellValue(guid);
            row.CreateCell(0).SetCellValue(rand.Next(32));
            row.CreateCell(0).SetCellValue(rand.Next(13));
            row.CreateCell(0).SetCellValue(date);
            row.CreateCell(0).SetCellValue((double)deci);
        }

        [Benchmark]
        public void CloseXmlRandomCells()
        {
            var rowNum = rand.Next(1, 1000000);

            closedXmlSheet.Cell($"A{rowNum}").Value = guidAsFormula;
            closedXmlSheet.Cell($"B{rowNum}").Value = guid;
            closedXmlSheet.Cell($"C{rowNum}").Value = rnd32;
            closedXmlSheet.Cell($"D{rowNum}").Value = rnd13;
            closedXmlSheet.Cell($"E{rowNum}").Value = date;
            closedXmlSheet.Cell($"F{rowNum}").Value = deci;
        }

        [Benchmark]
        public void EpplusRandomCells()
        {
            var rowNum = rand.Next(1, 1000000);

            epplusSheet.Cells[$"A{rowNum}"].Value = guidAsFormula;
            epplusSheet.Cells[$"B{rowNum}"].Value = guid;
            epplusSheet.Cells[$"C{rowNum}"].Value = rnd32;
            epplusSheet.Cells[$"D{rowNum}"].Value = rnd13;
            epplusSheet.Cells[$"E{rowNum}"].Value = date;
            epplusSheet.Cells[$"F{rowNum}"].Value = deci;
        }

        [GlobalCleanup]
        public void GlobalCleanup()
        {
            ixlSheet.WorkBook.Close();
            ixlOldSheet.WorkBook.Close();
            asposeCells.Dispose();
            npoiSheet.Workbook.Close();
            closedXmlSheet.Workbook.Dispose();
            epplusExcelPackage.Dispose();
        }

        private static string GetRandomDate(Random gen)
        {
            var start = new DateTime(1995, 1, 1);
            var range = (DateTime.Today - start).Days;

            return start.AddDays(gen.Next(range)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        private static decimal GetRandomDecimal(Random rng)
        {
            var scale = (byte)rng.Next(29);
            var sign = rng.Next(2) == 1;

            return new decimal(GetRandomRandInt(rng),
                GetRandomRandInt(rng),
                GetRandomRandInt(rng),
                sign,
                scale);
        }

        private static int GetRandomRandInt(Random rng)
        {
            var firstBits = rng.Next(0, 1 << 4) << 28;
            var lastBits = rng.Next(0, 1 << 28);

            return firstBits | lastBits;
        }
    }
}
