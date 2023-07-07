using BenchmarkDotNet.Attributes;
using System.IO;
using System.Text;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class SavingLargeFileBenchmark : BenchmarkBase
    {
        private readonly string _largeFileName = "LoadingTestFiles\\largeFile.pdf";
        private IronPdf.PdfDocument _ironPdfDocument;
        private iTextSharp.text.pdf.PdfReader _itsReader;
        private PdfSharp.Pdf.PdfDocument _pdfSharpDocument;

        [IterationSetup]
        public void IterationSetup()
        {
            _ironPdfDocument = IronPdf.PdfDocument.FromFile(_largeFileName);

            _itsReader = new iTextSharp.text.pdf.PdfReader(_largeFileName);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            _pdfSharpDocument = PdfSharp.Pdf.IO.PdfReader.Open(_largeFileName);
        }

        [Benchmark]
        public override void Iron_Pdf()
        {
            _ = _ironPdfDocument.SaveAs("Results\\IronPdfLargeFile.pdf");
        }

        [Benchmark]
        public override void Pdf_Sharp()
        {
            _pdfSharpDocument.Save("Results\\PdfSharpLargeFile.pdf");
        }

        [Benchmark]
        public override void ITextSharp()
        {
            var stamper = new iTextSharp.text.pdf.PdfStamper(
                _itsReader,
                new FileStream(
                    "Results\\ITextSharpLargeFile.pdf",
                    FileMode.Create));
            stamper.Close();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {
            _ironPdfDocument.Dispose();
            _ironPdfDocument = null;

            _itsReader.Close();
            _itsReader = null;

            _pdfSharpDocument.Close();
            _pdfSharpDocument = null;
        }
    }
}
