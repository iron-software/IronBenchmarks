using BenchmarkDotNet.Attributes;
using System.IO;
using System.Text;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SavingLargeFileBenchmark : BenchmarkBase
    {
        private readonly string largeFileName = "LoadingTestFiles\\largeFile.pdf";
        private IronPdf.PdfDocument ironPdfDocument;
        private iTextSharp.text.pdf.PdfReader itsReader;
        private PdfSharp.Pdf.PdfDocument pdfSharpDocument;

        [IterationSetup]
        public void IterationSetup()
        {
            ironPdfDocument = IronPdf.PdfDocument.FromFile(largeFileName);

            itsReader = new iTextSharp.text.pdf.PdfReader(largeFileName);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            pdfSharpDocument = PdfSharp.Pdf.IO.PdfReader.Open(largeFileName);
        }

        [Benchmark]
        public override void Iron_Pdf()
        {
            _ = ironPdfDocument.SaveAs("Results\\IronPdfLargeFile.pdf");
        }

        [Benchmark]
        public override void Pdf_Sharp()
        {
            pdfSharpDocument.Save("Results\\PdfSharpLargeFile.pdf");
        }

        [Benchmark]
        public override void ITextSharp()
        {
            var stamper = new iTextSharp.text.pdf.PdfStamper(
                itsReader,
                new FileStream(
                    "Results\\ITextSharpLargeFile.pdf",
                    FileMode.Create));
            stamper.Close();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            ironPdfDocument.Dispose();
            ironPdfDocument = null;

            itsReader.Close();
            itsReader = null;

            pdfSharpDocument.Close();
            pdfSharpDocument = null;
        }
    }
}
