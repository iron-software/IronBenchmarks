using BenchmarkDotNet.Attributes;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class LoadingLargeFileBenchmark : BenchmarkBase
    {
        private readonly string largeFileName = "LoadingTestFiles\\largeFile.pdf";

        [Benchmark]
        public override void Iron_Pdf()
        {
            _ = IronPdf.PdfDocument.FromFile(largeFileName);
        }

        //[Benchmark]
        public override void Iron_PdfOld()
        {
            _ = IronPdfOld.PdfDocument.FromFile(largeFileName);
        }
    }
}
