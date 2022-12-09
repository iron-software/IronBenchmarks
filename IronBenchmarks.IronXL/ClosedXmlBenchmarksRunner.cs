using ClosedXML.Excel;
using System;

namespace IronBenchmarks.IronXL
{
    internal class ClosedXmlBenchmarksRunner : BaseBenchmarksRunner<IXLWorksheet>
    {
        public ClosedXmlBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
        }

        protected override string BenchmarkRunnerName => typeof(ClosedXmlBenchmarksRunner).Name.Replace("BenchmarksRunner", "") ?? "ClosedXml";
        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(IXLCell))}";
        public override string LicenseKey { set => throw new NotImplementedException(); }

        protected override void PerformBenchmarkWork(Action<IXLWorksheet> benchmarkWork, string fileName, bool savingResultingFile)
        {
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Sheet1");

            benchmarkWork(worksheet);

            if (savingResultingFile)
            {
                workbook.SaveAs(fileName);
            }
        }
        protected override void LoadingBigFile(IXLWorksheet worksheet)
        {
            _ = new XLWorkbook(_largeFileName);
        }
        protected override void CreateRandomCells(IXLWorksheet worksheet)
        {
            var rand = new Random();
            for (var i = 1; i <= RandomCellsRowNumber; i++)
            {
                worksheet.Cell("A" + i).Value = $"=\"{Guid.NewGuid()}\"";
                worksheet.Cell("B" + i).Value = $"=\"{Guid.NewGuid()}\"";
                worksheet.Cell("C" + i).Value = Guid.NewGuid().ToString();
                worksheet.Cell("D" + i).Value = rand.Next(32);
                worksheet.Cell("E" + i).Value = $"=\"{Guid.NewGuid()}\"";
                worksheet.Cell("F" + i).Value = $"=\"{Guid.NewGuid()}\"";
                worksheet.Cell("G" + i).Value = Guid.NewGuid().ToString();
                worksheet.Cell("H" + i).Value = rand.Next(13);
                worksheet.Cell("I" + i).Value = GetRandomDate(rand);
                worksheet.Cell("J" + i).Value = GetRandomDate(rand);
                worksheet.Cell("K" + i).Value = Guid.NewGuid().ToString();
                worksheet.Cell("L" + i).Value = $"=\"{Guid.NewGuid()}\"";
                worksheet.Cell("M" + i).Value = Guid.NewGuid().ToString();
                worksheet.Cell("N" + i).Value = Guid.NewGuid().ToString();
                worksheet.Cell("O" + i).Value = GetRandomDecimal(rand);
                worksheet.Cell("P" + i).Value = GetRandomDecimal(rand);
            }
        }
        protected override void CreateDateCells(IXLWorksheet worksheet)
        {
            for (var i = 1; i < DateCellsNumber; i++)
            {
                worksheet.Cell("A" + i).Value = DateTime.Now;
            }
        }
        protected override void MakeStyleChanges(IXLWorksheet worksheet)
        {
            var range = worksheet.Range($"A1:O{StyleChangeRowNumber}");
            range.Value = _cellValue;

            var style = range.Style;

            style.Font.FontSize = 22;
            style.Alignment.SetVertical(XLAlignmentVerticalValues.Top);
            style.Alignment.SetHorizontal(XLAlignmentHorizontalValues.Right);
        }
        protected override void GenerateFormulas(IXLWorksheet worksheet)
        {
            var rnd = new Random();

            for (var i = 1; i <= GenerateFormulasRowNumber; i++)
            {
                for (var j = 1; j <= 10; j++)
                {
                    var cellA = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    var cellB = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    worksheet.Cell($"{_letters[j]}{i}").FormulaA1 = $"={cellA}/{cellB}";
                }
            }

            for (var i = GenerateFormulasRowNumber + 1; i <= GenerateFormulasRowNumber * 2; i++)
            {
                for (var j = 1; j <= 10; j++)
                {
                    worksheet.Cell($"{_letters[j]}{i}").Value = GetRandomRandInt(rnd);
                }
            }
        }

        protected override void SortRange(IXLWorksheet worksheet)
        {
            throw new NotImplementedException();
        }
    }
}
