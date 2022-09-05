using Microsoft.Office.Interop.Excel;
using System;
using System.IO;
using System.Reflection;
using Excel = Microsoft.Office.Interop.Excel;

namespace IronBenchmarks.IronXL
{
    internal class OfficeInteropBenchmarksRunner : BaseBenchmarksRunner<Range>
    {
        private readonly Application _excelApp = new Application();

        public OfficeInteropBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
        }

        public void QuitExcelApp()
        {
            _excelApp.Quit();
        }

        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(Range))}";
        protected override string BenchmarkRunnerName => typeof(OfficeInteropBenchmarksRunner).Name.Replace("BenchmarksRunner", "") ?? "Office Interop";
        protected override void PerformBenchmarkWork(Action<Range> benchmarkWork, string fileName, bool savingResultingFile)
        {
            var workbook = _excelApp.Workbooks.Add();
            var cells = ((Worksheet)workbook.ActiveSheet).Cells;

            benchmarkWork(cells);

            if (savingResultingFile)
            {
                workbook.SaveAs(
                    Filename: Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + fileName,
                    FileFormat: XlFileFormat.xlWorkbookDefault,
                    Password: Type.Missing,
                    WriteResPassword: Type.Missing,
                    ReadOnlyRecommended: false,
                    CreateBackup: false,
                    AccessMode: XlSaveAsAccessMode.xlNoChange,
                    ConflictResolution: Type.Missing,
                    AddToMru: Type.Missing,
                    TextCodepage: Type.Missing,
                    TextVisualLayout: Type.Missing,
                    Local: Type.Missing);
            }

            workbook.Close(false);
        }
        protected override void LoadingBigFile(Range cells)
        {
            _ = _excelApp.Workbooks.Open(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + _largeFileName);
        }
        protected override void CreateRandomCells(Range cells)
        {
            var rand = new Random();
            for (var i = 1; i <= RandomCellsRowNumber; i++)
            {
                cells[i, 1] = $"=\"{Guid.NewGuid()}\"";
                cells[i, 2] = $"=\"{Guid.NewGuid()}\"";
                cells[i, 3] = Guid.NewGuid().ToString();
                cells[i, 4] = rand.Next(32);
                cells[i, 5] = $"=\"{Guid.NewGuid()}\"";
                cells[i, 6] = $"=\"{Guid.NewGuid()}\"";
                cells[i, 7] = Guid.NewGuid().ToString();
                cells[i, 8] = rand.Next(13);
                cells[i, 9] = GetRandomDate(rand);
                cells[i, 10] = GetRandomDate(rand);
                cells[i, 11] = Guid.NewGuid().ToString();
                cells[i, 12] = $"=\"{Guid.NewGuid()}\"";
                cells[i, 13] = Guid.NewGuid().ToString();
                cells[i, 14] = Guid.NewGuid().ToString();
                cells[i, 15] = GetRandomDecimal(rand);
                cells[i, 16] = GetRandomDecimal(rand);
            }
        }
        protected override void CreateDateCells(Range cells)
        {
            for (var i = 1; i < DateCellsNumber; i++)
            {
                var cell = (Range)cells[i, 1];
                cell.Value = DateTime.Now;
            }

            cells.NumberFormat = "DD/MM/YYYY";
        }
        protected override void MakeStyleChanges(Range cells)
        {
            var range = cells.Range["A1", $"O{StyleChangeRowNumber}"];
            range.Value = _cellValue;

            range.Font.Size = 22;
            range.VerticalAlignment = XlVAlign.xlVAlignTop;
            range.HorizontalAlignment = XlHAlign.xlHAlignRight;
        }
        protected override void GenerateFormulas(Range cells)
        {
            var rnd = new Random();

            for (var i = 1; i <= GenerateFormulasRowNumber; i++)
            {
                for (var j = 1; j <= 10; j++)
                {
                    var cellA = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    var cellB = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    cells[i, j] = $"={cellA}/{cellB}";
                }
            }

            for (var i = GenerateFormulasRowNumber + 1; i <= GenerateFormulasRowNumber * 2; i++)
            {
                for (var j = 1; j <= 10; j++)
                {
                    cells[i, j] = GetRandomRandInt(rnd);
                }
            }
        }
    }
}
