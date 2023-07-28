using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class RandomCellsBenchmark : SheetOperationsBenchmarkBase
    {
        private readonly string _guid = Guid.NewGuid().ToString();

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            IronXlSheet[$"A2"].Value = _guid;
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            Iron_XlOldSheet[$"A2"].Value = _guid;
        }

        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            AsposeCells[$"A2"].Value = _guid;
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            NpoiSheet.CreateRow(1).CreateCell(0).SetCellValue(_guid);
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            ClosedXmlSheet.Cell($"A2").Value = _guid;
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            EpplusSheet.Cells[$"A2"].Value = _guid;
        }
    }
}
