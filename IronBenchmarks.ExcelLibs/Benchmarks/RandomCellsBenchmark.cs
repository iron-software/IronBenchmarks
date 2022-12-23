using BenchmarkDotNet.Attributes;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class RandomCellsBenchmark : BenchmarkBase
    {
        private readonly string guid = Guid.NewGuid().ToString();

        [Benchmark]
        public override void IronXl()
        {
            IxlSheet[$"A2"].Value = guid;
        }

        [Benchmark]
        public override void IronXlOld()
        {
            IxlOldSheet[$"A2"].Value = guid;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            AsposeCells[$"A2"].Value = guid;
        }

        [Benchmark]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(1).CreateCell(0).SetCellValue(guid);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").Value = guid;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = guid;
        }
    }
}
