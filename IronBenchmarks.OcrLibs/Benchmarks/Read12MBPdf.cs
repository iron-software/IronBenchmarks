using BenchmarkDotNet.Attributes;
using IronOcr;

namespace IronBenchmarks.OcrLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class Read12MBPdf : BenchmarkBase
    {
        [Benchmark]
        public override void Aspose_Ocr()
        {
            Aspose.OCR.AsposeOcr recognitionEngine = new Aspose.OCR.AsposeOcr();
            var source = new Aspose.OCR.OcrInput(Aspose.OCR.InputType.PDF);
            source.Add(@"TestPdfs\linux_commands.pdf");

            List<Aspose.OCR.RecognitionResult> results = recognitionEngine.Recognize(source);

            foreach (Aspose.OCR.RecognitionResult result in results)
            {
                _ = result.RecognitionText;
            }
        }

        [Benchmark]
        public override void Iron_Ocr()
        {
            var input = new OcrInput();
            input.LoadPdf("TestPdfs/linux_commands.pdf");
            var ocr = new IronTesseract();
            OcrResult result = ocr.Read(input);
            _ = result.Text;
        }
    }
}
