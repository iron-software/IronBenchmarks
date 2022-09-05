using IronPdf;

namespace Benchmarks.IronPdfBench
{
    internal class CurrentIronPdfBenchmarksRunner : BaseBenchmarksRunner
    {
        public CurrentIronPdfBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
        }

        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(License))}";

        protected override string BenchmarkRunnerName => typeof(CurrentIronPdfBenchmarksRunner).Name.Replace("BenchmarksRunner", "") ?? "IronPdf";

        public override void Generate10Pdf()
        {
            var renderer = new ChromePdfRenderer();

            for (int i = 0; i < 10; i++)
            {
                _ = renderer.RenderHtmlAsPdf(" <h1> ~Hello World~ </h1> Made with IronPDF!");
            }
        }

        public override void Generate10PdfSaveFiles()
        {
            var renderer = new ChromePdfRenderer();

            for (int i = 0; i < 10; i++)
            {
                var pdf = renderer.RenderHtmlAsPdf(" <h1> ~Hello World~ </h1> Made with IronPDF!");
                pdf.SaveAs($"{ResultsFolderName}\\CurrentIronPdf{i}.pdf");
            }
        }
    }
}
