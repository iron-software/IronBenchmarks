using BenchmarkDotNet.Attributes;
using IronOcr;

namespace IronBenchmarks.OcrLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class ReadImage : BenchmarkBase
    {
        [Benchmark]
        public override void Aspose_Ocr()
        {
            Aspose.OCR.AsposeOcr recognitionEngine = new Aspose.OCR.AsposeOcr();
            var source = new Aspose.OCR.OcrInput(Aspose.OCR.InputType.SingleImage);
            source.Add(@"TestImages\stock_gs200.jpg");

            List<Aspose.OCR.RecognitionResult> results = recognitionEngine.Recognize(source);

            foreach (Aspose.OCR.RecognitionResult result in results)
            {
                _ = result.RecognitionText;
            }
        }

        [Benchmark]
        public override void Iron_Ocr()
        {
            var input = new IronOcr.OcrInput();
            input.LoadImage(@"TestImages\stock_gs200.jpg");

            var ironTesseract = new IronTesseract();
            OcrResult result = ironTesseract.Read(input);

            foreach (OcrResult.Page page in result.Pages)
            {
                _ = page.Text;
            }
        }
    }
}
