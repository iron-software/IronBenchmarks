using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace Benchmarks.IronXL
{
    internal class NpoiBenchmarkRunner : IronXlBenchmarksRunner<ISheet>
    {
        public NpoiBenchmarkRunner(string resultsFolder) : base(resultsFolder)
        {
        }

        protected override string BenchmarkRunnerName => typeof(NpoiBenchmarkRunner).Name.Replace("BenchmarkRunner", "") ?? "NPOI";
        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(ICell))}";
        protected override void PerformBenchmarkWork(Action<ISheet> benchmarkWork, string fileName, bool savingResultingFile)
        {
            var workbook = new XSSFWorkbook();
            var sheet = workbook.CreateSheet();

            benchmarkWork(sheet);

            if (savingResultingFile)
            {
                workbook.Write(File.Create(fileName));
            }
        }
        protected override void LoadingBigFile(ISheet worksheet)
        {
            _ = new XSSFWorkbook(_largeFileName);
        }
        protected override void CreateRandomCells(ISheet worksheet)
        {
            var rand = new Random();
            for (int i = 0; i <= RandomCellsRowNumber; i++)
            {
                var row = worksheet.CreateRow(i);
                row.CreateCell(0).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(1).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(2).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(3).SetCellValue(rand.Next(32));
                row.CreateCell(4).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(5).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(6).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(7).SetCellValue(rand.Next(13));
                row.CreateCell(8).SetCellValue(GetRandomDate(rand));
                row.CreateCell(9).SetCellValue(GetRandomDate(rand));
                row.CreateCell(10).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(11).SetCellValue($"=\"{Guid.NewGuid()}\"");
                row.CreateCell(12).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(13).SetCellValue(Guid.NewGuid().ToString());
                row.CreateCell(14).SetCellValue((double)GetRandomDecimal(rand));
                row.CreateCell(15).SetCellValue((double)GetRandomDecimal(rand));
            }
        }
        protected override void CreateDateCells(ISheet worksheet)
        {
            var style = worksheet.Workbook.CreateCellStyle();
            style.DataFormat = worksheet.Workbook.CreateDataFormat().GetFormat("dd/MM/yyyy");

            for (int i = 0; i < DateCellsNumber; i++)
            {
                var cell = worksheet.CreateRow(i).CreateCell(0);
                cell.SetCellValue(DateTime.Now);
                cell.CellStyle = style;
            }
        }
        protected override void MakeStyleChanges(ISheet worksheet)
        {
            var font = worksheet.Workbook.CreateFont();
            font.FontHeightInPoints = 22;

            var style = worksheet.Workbook.CreateCellStyle();
            style.SetFont(font);
            style.VerticalAlignment = VerticalAlignment.Top;
            style.Alignment = HorizontalAlignment.Right;

            for (int i = 0; i < StyleChangeRowNumber; i++)
            {
                var row = worksheet.CreateRow(i);
                for (int j = 0; j < 15; j++)
                {
                    var cell = row.CreateCell(j);
                    cell.SetCellValue(_cellValue);
                    cell.CellStyle = style;
                }
            }
        }
        protected override void GenerateFormulas(ISheet worksheet)
        {
            var rnd = new Random();

            for (int i = 0; i < GenerateFormulasRowNumber; i++)
            {
                var row = worksheet.CreateRow(i);
                for (int j = 0; j < 10; j++)
                {
                    string cellA = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    string cellB = $"{_letters[rnd.Next(1, 10)]}{rnd.Next(GenerateFormulasRowNumber + 1, GenerateFormulasRowNumber * 2)}";
                    row.CreateCell(j).SetCellFormula($"{cellA}/{cellB}");
                }
            }

            for (int i = GenerateFormulasRowNumber; i < GenerateFormulasRowNumber * 2; i++)
            {
                var row = worksheet.CreateRow(i);
                for (int j = 0; j < 10; j++)
                {
                    row.CreateCell(j).SetCellValue(GetRandomRandInt(rnd));
                }
            }
        }
    }
}
