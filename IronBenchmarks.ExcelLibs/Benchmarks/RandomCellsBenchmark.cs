using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class RandomCellsBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly string _guid = Guid.NewGuid().ToString();

        [Benchmark]
        public override void IronXl()
        {
            IronXlSheet[$"A2"].Value = _guid;
        }

        [Benchmark]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet[$"A2"].Value = _guid;
        }

        [Benchmark(Baseline = true)]
        public override void Aspose()
        {
            AsposeCells[$"A2"].Value = _guid;
        }

        [Benchmark]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(1).CreateCell(0).SetCellValue(_guid);
        }

        [Benchmark]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").Value = _guid;
        }

        [Benchmark]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = _guid;
        }
    }
}
