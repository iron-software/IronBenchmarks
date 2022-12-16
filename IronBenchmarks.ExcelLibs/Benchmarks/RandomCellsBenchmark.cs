using BenchmarkDotNet.Attributes;
using System;
using System.Globalization;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class RandomCellsBenchmark : BenchmarkBase
    {
        private readonly Random rand = new Random();
        private readonly string guidAsFormula = $"=\"{Guid.NewGuid()}\"";
        private readonly string guid = Guid.NewGuid().ToString();
        private readonly string date;
        private readonly decimal deci;
        private readonly int rnd32;
        private readonly int rnd13;

        public RandomCellsBenchmark() : base()
        {
            date = GetRandomDate(rand);
            deci = GetRandomDecimal(rand);
            rnd32 = rand.Next(32);
            rnd13 = rand.Next(13);
        }

        [Benchmark]
        public override void IronXl()
        {
            var rowNum = rand.Next(1, 1000000);

            IxlSheet[$"A{rowNum}"].Value = guidAsFormula;
            IxlSheet[$"B{rowNum}"].Value = guid;
            IxlSheet[$"C{rowNum}"].Value = rnd32;
            IxlSheet[$"D{rowNum}"].Value = rnd13;
            IxlSheet[$"E{rowNum}"].Value = date;
            IxlSheet[$"F{rowNum}"].Value = deci;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            var rowNum = rand.Next(1, 1000000);

            IxlOldSheet[$"A{rowNum}"].Value = guidAsFormula;
            IxlOldSheet[$"B{rowNum}"].Value = guid;
            IxlOldSheet[$"C{rowNum}"].Value = rnd32;
            IxlOldSheet[$"D{rowNum}"].Value = rnd13;
            IxlOldSheet[$"E{rowNum}"].Value = date;
            IxlOldSheet[$"F{rowNum}"].Value = deci;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            var rowNum = rand.Next(1, 1000000);

            AsposeCells[$"A{rowNum}"].Value = guidAsFormula;
            AsposeCells[$"B{rowNum}"].Value = guid;
            AsposeCells[$"C{rowNum}"].Value = rnd32;
            AsposeCells[$"D{rowNum}"].Value = rnd13;
            AsposeCells[$"E{rowNum}"].Value = date;
            AsposeCells[$"F{rowNum}"].Value = deci;
        }

        [Benchmark]
        public override void Npoi()
        {
            var rowNum = rand.Next(0, 1000000);

            var row = NpoiSheet.CreateRow(rowNum);

            row.CreateCell(0).SetCellValue(guidAsFormula);
            row.CreateCell(0).SetCellValue(guid);
            row.CreateCell(0).SetCellValue(rand.Next(32));
            row.CreateCell(0).SetCellValue(rand.Next(13));
            row.CreateCell(0).SetCellValue(date);
            row.CreateCell(0).SetCellValue((double)deci);
        }

        [Benchmark]
        public override void CloseXml()
        {
            var rowNum = rand.Next(1, 1000000);

            ClosedXmlSheet.Cell($"A{rowNum}").Value = guidAsFormula;
            ClosedXmlSheet.Cell($"B{rowNum}").Value = guid;
            ClosedXmlSheet.Cell($"C{rowNum}").Value = rnd32;
            ClosedXmlSheet.Cell($"D{rowNum}").Value = rnd13;
            ClosedXmlSheet.Cell($"E{rowNum}").Value = date;
            ClosedXmlSheet.Cell($"F{rowNum}").Value = deci;
        }

        [Benchmark]
        public override void Epplus()
        {
            var rowNum = rand.Next(1, 1000000);

            EpplusSheet.Cells[$"A{rowNum}"].Value = guidAsFormula;
            EpplusSheet.Cells[$"B{rowNum}"].Value = guid;
            EpplusSheet.Cells[$"C{rowNum}"].Value = rnd32;
            EpplusSheet.Cells[$"D{rowNum}"].Value = rnd13;
            EpplusSheet.Cells[$"E{rowNum}"].Value = date;
            EpplusSheet.Cells[$"F{rowNum}"].Value = deci;
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
