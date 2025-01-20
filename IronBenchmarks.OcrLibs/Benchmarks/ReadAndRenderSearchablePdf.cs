using BenchmarkDotNet.Attributes;
using IronOcr;

namespace IronBenchmarks.OcrLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class ReadAndRenderSearchablePdf : BenchmarkBase
    {
        [Benchmark]
        public override void Aspose_Ocr()
        {
            Aspose.OCR.AsposeOcr recognitionEngine = new Aspose.OCR.AsposeOcr();
            var source = new Aspose.OCR.OcrInput(Aspose.OCR.InputType.TIFF);
            source.Add(@"TestMultiPageImages\test_dw_10.tif");

            List<Aspose.OCR.RecognitionResult> results = recognitionEngine.Recognize(source);

            foreach (Aspose.OCR.RecognitionResult result in results)
            {
                _ = result.RecognitionText;
            }

            Aspose.OCR.AsposeOcr.SaveMultipageDocument("aspose_large_tiff.pdf", Aspose.OCR.SaveFormat.Pdf, results);
        }

        [Benchmark]
        public override void Iron_Ocr()
        {
            var input = new IronOcr.OcrInput();
            input.LoadImage(@"TestMultiPageImages\test_dw_10.tif");

            var ironTesseract = new IronTesseract();
            OcrResult result = ironTesseract.Read(input);

            foreach (OcrResult.Page page in result.Pages)
            {
                _ = page.Text;
            }

            result.SaveAsSearchablePdf("iron_large_tiff.pdf");
        }
    }
}
