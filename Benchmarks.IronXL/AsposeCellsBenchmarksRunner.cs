using Aspose.Cells;
using System;

namespace Benchmarks.IronXL
{
    internal class AsposeCellsBenchmarksRunner : IronXlBenchmarksRunner<Cells>
    {
        public AsposeCellsBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
        }

        protected override string BenchmarkRunnerName => typeof(AsposeCellsBenchmarksRunner).Name.Replace("BenchmarksRunner", "") ?? "Aspose Cells";
        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(Cell))}";
        protected override void PerformBenchmarkWork(Action<Cells> benchmarkWork, string fileName, bool savingResultingFile)
        {
            var workbook = new Workbook();
            var cells = workbook.Worksheets[0].Cells;

            benchmarkWork(cells);

            if (savingResultingFile)
            {
                workbook.Save(fileName);
            }
        }
        protected override void LoadingBigFile(Cells cells)
        {
            _ = new Workbook(_largeFileName);
        }
        protected override void CreateRandomCells(Cells cells)
        {
            var rand = new Random();
            for (int i = 1; i <= RandomCellsRowNumber; i++)
            {
                cells["A" + i].Value = $"=\"{Guid.NewGuid()}\"";
                cells["B" + i].Value = $"=\"{Guid.NewGuid()}\"";
                cells["C" + i].Value = Guid.NewGuid().ToString();
                cells["D" + i].Value = rand.Next(32);
                cells["E" + i].Value = $"=\"{Guid.NewGuid()}\"";
                cells["F" + i].Value = $"=\"{Guid.NewGuid()}\"";
                cells["G" + i].Value = Guid.NewGuid().ToString();
                cells["H" + i].Value = rand.Next(13);
                cells["I" + i].Value = GetRandomDate(rand);
                cells["J" + i].Value = GetRandomDate(rand);
                cells["K" + i].Value = Guid.NewGuid().ToString();
                cells["L" + i].Value = $"=\"{Guid.NewGuid()}\"";
                cells["M" + i].Value = Guid.NewGuid().ToString();
                cells["N" + i].Value = Guid.NewGuid().ToString();
                cells["O" + i].Value = GetRandomDecimal(rand);
                cells["P" + i].Value = GetRandomDecimal(rand);
            }
        }
        protected override void CreateDateCells(Cells cells)
        {
            var style = new CellsFactory().CreateStyle();
            style.Number = 15;

            for (int i = 1; i < DateCellsNumber; i++)
            {
                var cell = cells["A" + i];
                cell.PutValue(DateTime.Now);
                cell.SetStyle(style);
            }
        }
        protected override void MakeStyleChanges(Cells cells)
        {
            var style = new CellsFactory().CreateStyle();
            style.Font.Size = 22;
            style.VerticalAlignment = TextAlignmentType.Top;
            style.HorizontalAlignment = TextAlignmentType.Right;

            cells.InsertRows(1, StyleChangeRowNumber);

            var range = cells.CreateRange($"A1:O{StyleChangeRowNumber}");
            range.Value = _cellValue;

            range.SetStyle(style);
        }
        protected override void GenerateFormulas(Cells cells)
        {
            var rnd = new Random();

            for (int i = 1; i <= GenerateFormulasRowNumber; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    string cellA = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    string cellB = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    cells[$"{_letters[j]}{i}"].Formula = $"={cellA}/{cellB}";
                }
            }

            for (int i = GenerateFormulasRowNumber + 1; i <= GenerateFormulasRowNumber * 2; i++)
            {
                for (int j = 1; j <= 10; j++)
                {
                    cells[$"{_letters[j]}{i}"].Value = GetRandomRandInt(rnd);
                }
            }
        }
    }
}
