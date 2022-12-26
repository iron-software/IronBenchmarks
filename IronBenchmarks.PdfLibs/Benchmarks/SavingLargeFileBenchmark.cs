using BenchmarkDotNet.Attributes;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SavingLargeFileBenchmark
    {
        private readonly string largeFileName = "LoadingTestFiles\\largeFile.pdf";
        private IronPdf.PdfDocument iPdf;
        private IronPdfOld.PdfDocument iOldPdf;


        public SavingLargeFileBenchmark()
        {
            BenchmarkBase.SetupLicenses();
            BenchmarkBase.EnsureResultsFolderExists();
        }

        [IterationSetup]
        public void IterationSetup()
        {
            iPdf = IronPdf.PdfDocument.FromFile(largeFileName);
            //iOldPdf = IronPdfOld.PdfDocument.FromFile(largeFileName);
        }

        [Benchmark]
        public void Iron_Pdf()
        {
            _ = iPdf.SaveAs("Results\\IronPdfLargeFile.pdf");
        }

        //[Benchmark]
        public void Iron_PdfOld()
        {
            _ = iOldPdf.SaveAs("Results\\IronPdfLargeFile.pdf");
        }
    }
}
