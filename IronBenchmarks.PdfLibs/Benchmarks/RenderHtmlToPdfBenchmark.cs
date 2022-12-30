using BenchmarkDotNet.Attributes;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System;
using System.IO;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class RenderHtmlToPdfBenchmark : BenchmarkBase
    {
        private readonly string html = "<h1> ~Hello World~ </h1> Made with IronPDF!";

        [IterationSetup]
        public void IterationSetup()
        {

        }

        [Benchmark]
        public override void Iron_Pdf()
        {
            var pdfRenderer = new IronPdf.ChromePdfRenderer();
            _ = pdfRenderer.RenderHtmlAsPdf(html);
        }

        [Benchmark]
        public override void ITextSharp()
        {
            var stringReader = new StringReader(html);
            var document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            var pdfWriter = PdfWriter.GetInstance(document, new MemoryStream());
            document.Open();
            XMLWorkerHelper.GetInstance().ParseXHtml(pdfWriter, document, stringReader);
            document.Close();
        }

        [Benchmark]
        public override void Pdf_Sharp()
        {
            throw new NotImplementedException();
        }

        [IterationCleanup]
        public void IterationCleanup()
        {

        }
    }
}
