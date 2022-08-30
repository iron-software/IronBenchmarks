using Benchmarks.BenchmarkRunners;
using Benchmarks.Configuration;
using IronBarCodeOld;

namespace Benchmarks.App.BenchmarkRunners
{
    internal class CurrentBarCodeBenchmarkRunner : BenchmarkRunner
    {
        public CurrentBarCodeBenchmarkRunner(IAppConfig appConfig) : base(appConfig) { }

        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(BarcodeEncoding))}";

        protected override string BenchmarkRunnerName => typeof(CurrentBarCodeBenchmarkRunner).Name.Replace("BenchmarkRunner", "") ?? "Current Barcode";

        protected override void Generate500QrCodes(bool saveResults)
        {
            for (var i = 0; i < 500; i++)
            {
                var qr = QRCodeWriter.CreateQrCode(Guid.NewGuid().ToString(), 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);

                if (saveResults)
                {
                    qr.SaveAsPng($"{_resultsFolderName}\\{BenchmarkRunnerName}QR{i}.png");
                }
            }
        }
    }
}
