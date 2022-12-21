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
        private readonly string guid = Guid.NewGuid().ToString();

        [Benchmark]
        public override void IronXl()
        {
            var rowNum = rand.Next(1, 1000000);

            IxlSheet[$"A{rowNum}"].Value = guid;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            var rowNum = rand.Next(1, 1000000);

            IxlOldSheet[$"A{rowNum}"].Value = guid;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            var rowNum = rand.Next(1, 1000000);

            AsposeCells[$"A{rowNum}"].Value = guid;
        }

        [Benchmark]
        public override void Npoi()
        {
            var rowNum = rand.Next(0, 1000000);

            var row = NpoiSheet.CreateRow(rowNum);

            row.CreateCell(0).SetCellValue(guid);
        }

        [Benchmark]
        public override void CloseXml()
        {
            var rowNum = rand.Next(1, 1000000);

            ClosedXmlSheet.Cell($"A{rowNum}").Value = guid;
        }

        [Benchmark]
        public override void Epplus()
        {
            var rowNum = rand.Next(1, 1000000);

            EpplusSheet.Cells[$"A{rowNum}"].Value = guid;
        }
    }
}
