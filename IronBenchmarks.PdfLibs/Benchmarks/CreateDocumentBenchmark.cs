using BenchmarkDotNet.Attributes;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreateDocumentBenchmark : BenchmarkBase
    {
        [Benchmark]
        public override void Iron_Pdf()
        {
            _ = IronPdfRenderer.RenderHtmlAsPdf(" <h1> ~Hello World~ </h1> Made with IronPDF!");
        }

        //[Benchmark]
        public override void Iron_PdfOld()
        {
            _ = IronPdfOldRenderer.RenderHtmlAsPdf(" <h1> ~Hello World~ </h1> Made with IronPDF!");
        }
    }
}
