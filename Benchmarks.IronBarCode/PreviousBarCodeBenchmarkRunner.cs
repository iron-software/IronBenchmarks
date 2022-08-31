using IronBarCodeOld;
using System;

namespace Benchmarks.IronBarCode
{
    public class PreviousBarCodeBenchmarkRunner : BarcodeBenchmarksRunner
    {
        public PreviousBarCodeBenchmarkRunner(string resultFolder) : base(resultFolder) { }

        public override string NameAndVersion => $"{BenchmarkRunnerName} v.{GetAssemblyVersion(typeof(BarcodeEncoding))}";

        protected override string BenchmarkRunnerName => typeof(PreviousBarCodeBenchmarkRunner).Name.Replace("BenchmarkRunner", "") ?? "Previous Barcode";

        public override void Generate500QrCodes()
        {
            for (var i = 0; i < 500; i++)
            {
                QRCodeWriter.CreateQrCode(Guid.NewGuid().ToString(), 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);
            }
        }

        public override void Generate500QrCodesSaveFiles()
        {
            for (var i = 0; i < 500; i++)
            {
                var qr = QRCodeWriter.CreateQrCode(Guid.NewGuid().ToString(), 500, QRCodeWriter.QrErrorCorrectionLevel.Medium);

                qr.SaveAsPng($"{resultsFolderName}\\{BenchmarkRunnerName}QR{i}.png");
            }
        }
    }
}
