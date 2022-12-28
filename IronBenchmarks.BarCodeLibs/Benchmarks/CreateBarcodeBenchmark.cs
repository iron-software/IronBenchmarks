using BenchmarkDotNet.Attributes;
using System;

namespace IronBenchmarks.BarCodeLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreateBarcodeBenchmark : BenchmarkBase
    {
        private readonly string text = Guid.NewGuid().ToString();

        [Benchmark]
        public override void Iron_BarCode()
        {
            IronBarCode.QRCodeWriter.CreateQrCode(
                text,
                500,
                IronBarCode.QRCodeWriter.QrErrorCorrectionLevel.Medium);
        }

        [Benchmark]
        public override void Iron_BarCodeOld()
        {
            IronBarCodeOld.QRCodeWriter.CreateQrCode(
                text,
                500,
                IronBarCodeOld.QRCodeWriter.QrErrorCorrectionLevel.Medium);
        }
    }
}
