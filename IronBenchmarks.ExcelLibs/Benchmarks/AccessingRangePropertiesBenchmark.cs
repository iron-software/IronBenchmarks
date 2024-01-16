using Aspose.Cells;
using BenchmarkDotNet.Attributes;
using IronBenchmarks.ExcelLibs.Benchmarks.Bases;
using IronBenchmarks.ExcelLibs.Config;
using System;

namespace IronBenchmarks.ExcelLibs.Benchmarks
{
    [MemoryDiagnoser]
    [Config(typeof(ExcelConfig))]
    public class AccessingRangePropertiesBenchmark : SheetOperationsBenchmarkBase
    {
        [Benchmark(Baseline = true)]
        [BenchmarkCategory("Aspose")]
        public override void Aspose()
        {
            Aspose.Cells.Range range = AsposeCells.CreateRange("A1", "CV100");

            foreach (Cell cell in range)
            {
                _ = cell.GetStyle().Font;
            }
        }

        [Benchmark]
        [BenchmarkCategory("ClosedXml")]
        public override void ClosedXml()
        {
            _ = ClosedXmlSheet.Range("A1:CV1000").Style.Font;
        }

        [Benchmark]
        [BenchmarkCategory("Epplus")]
        public override void Epplus()
        {
            _ = EpplusSheet.Cells["A1:CV1000"].Style.Font;
        }

        [Benchmark]
        [BenchmarkCategory("IronXl")]
        public override void IronXl()
        {
            _ = IronXlSheet["A1:CV100"].Style.Font;
        }

        [Benchmark]
        [BenchmarkCategory("Iron_XlOld")]
        public override void Iron_XlOld()
        {
            _ = Iron_XlOldSheet["A1:CV100"].Style.Font;
        }

        [Benchmark]
        [BenchmarkCategory("Npoi")]
        public override void Npoi()
        {
            throw new NotImplementedException();
        }
    }
}
