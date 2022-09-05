using Benchmarks.Runner;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Benchmarks.IronXL
{
    public abstract class BaseBenchmarksRunner<T> : BenchmarksRunner
    {
        public int DateCellsNumber = 80000;
        public int RandomCellsRowNumber = 20000;
        public int StyleChangeRowNumber = 6000;
        public int GenerateFormulasRowNumber = 1000;

        protected static string _randomCellsFileNameTemplate = "{0}\\{1}_RandomCells.xlsx";
        protected static string _dateCellsFileNameTemplate = "{0}\\{1}_DateCells.xlsx";
        protected static string _styleChangeFileNameTemplate = "{0}\\{1}_StyleChange.xlsx";
        protected static string _loadingLargeFileFileNameTemplate = "{0}\\{1}_LoadingBigFile.xlsx";
        protected static string _generateFormulasFileNameTemplate = "{0}\\{1}_GenerateFormulas.xlsx";
        protected static string _cellValue = "Cell";
        protected static string _largeFileName = "LoadingTestFiles\\LoadingTest.xlsx";

        protected static readonly Dictionary<int, string> _letters = new Dictionary<int, string>()
        {
            {1, "A"},
            {2, "B"},
            {3, "C"},
            {4, "D"},
            {5, "E"},
            {6, "F"},
            {7, "G"},
            {8, "H"},
            {9, "I"},
            {10, "J"},
            {11, "K"},
            {12, "L"},
            {13, "M"},
            {14, "N"},
            {15, "O"},
            {16, "P"},
            {17, "Q"},
            {18, "R"},
            {19, "S"},
            {20, "T"},
            {21, "U"},
            {22, "V"},
            {23, "W"},
            {24, "X"},
            {25, "Y"},
            {26, "Z"}
        };


        protected string LoadingLargeFileName => string.Format(CultureInfo.InvariantCulture, _loadingLargeFileFileNameTemplate, ResultsFolderName, BenchmarkRunnerName);

        public BaseBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
            BenchmarkMethods = new Dictionary<string, string>()
            {
                { "RandomCellsBenchmark", "Create 320K cells\nwith random data" },
                { "RandomCellsBenchmarkSaveFile", "Create 320K cells\nwith random data (save file)" },
                { "DateCellsBenchmark", "Create 80K cells\nwith Date data" },
                { "DateCellsBenchmarkSaveFile", "Create 80K cells\nwith Date data (save file)" },
                { "StyleChangesBenchmark", "Create 90K cells\nand change the styles" },
                { "StyleChangesBenchmarkSaveFile", "Create 90K cells and\nchange the styles (save file)" },
                { "LoadingBigFileBenchmark", "Loading a file with\n640K unique cells" },
                { "GenerateFormulasBenchmark", "Generating 100K cells with formulas\nthat depend on other cells" },
                { "GenerateFormulasBenchmarkSaveFile", "Generating 100K cells with formulas\nthat depend on other cells (save file)" },
            };
        }

        public void RandomCellsBenchmark()
        {
            PerformBenchmarkWork(CreateRandomCells);
        }
        public void RandomCellsBenchmarkSaveFile()
        {
            var randomCellsFileName = GetResultFileName(_randomCellsFileNameTemplate);

            PerformBenchmarkWork(CreateRandomCells, randomCellsFileName, true);
        }
        public void DateCellsBenchmark()
        {
            PerformBenchmarkWork(CreateDateCells);
        }
        public void DateCellsBenchmarkSaveFile()
        {
            var dateCellsFileName = GetResultFileName(_dateCellsFileNameTemplate);

            PerformBenchmarkWork(CreateDateCells, dateCellsFileName, true);
        }
        public void StyleChangesBenchmark()
        {
            PerformBenchmarkWork(MakeStyleChanges);
        }
        public void StyleChangesBenchmarkSaveFile()
        {
            var styleChangeFileName = GetResultFileName(_styleChangeFileNameTemplate);

            PerformBenchmarkWork(MakeStyleChanges, styleChangeFileName, true);
        }
        public void LoadingBigFileBenchmark()
        {
            PerformBenchmarkWork(LoadingBigFile);
        }
        public void GenerateFormulasBenchmark()
        {
            PerformBenchmarkWork(GenerateFormulas);
        }
        public void GenerateFormulasBenchmarkSaveFile()
        {
            var genrateFormulasFileName = GetResultFileName(_generateFormulasFileNameTemplate);

            PerformBenchmarkWork(GenerateFormulas, genrateFormulasFileName, true);
        }

        protected abstract void PerformBenchmarkWork(Action<T> benchmarkWork, string fileName, bool savingResultingFile);
        protected abstract void LoadingBigFile(T worksheet);
        protected abstract void CreateRandomCells(T worksheet);
        protected abstract void CreateDateCells(T worksheet);
        protected abstract void MakeStyleChanges(T worksheet);
        protected abstract void GenerateFormulas(T worksheet);

        private void PerformBenchmarkWork(Action<T> benchmarkWork)
        {
            PerformBenchmarkWork(benchmarkWork, "", false);
        }

        protected static string GetRandomDate(Random gen)
        {
            DateTime start = new DateTime(1995, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(gen.Next(range)).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
        }

        protected static decimal GetRandomDecimal(Random rng)
        {
            byte scale = (byte)rng.Next(29);
            bool sign = rng.Next(2) == 1;
            return new decimal(GetRandomRandInt(rng),
                GetRandomRandInt(rng),
                GetRandomRandInt(rng),
                sign,
                scale);
        }

        protected static int GetRandomRandInt(Random rng)
        {
            int firstBits = rng.Next(0, 1 << 4) << 28;
            int lastBits = rng.Next(0, 1 << 28);
            return firstBits | lastBits;
        }

        private string GetResultFileName(string template)
        {
            return string.Format(CultureInfo.InvariantCulture, template, ResultsFolderName, BenchmarkRunnerName);
        }
    }
}
