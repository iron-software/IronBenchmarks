﻿using BenchmarkDotNet.Attributes;
using System;

namespace IronBenchmarks.BarCodeLibs.Benchmarks
{
    [ShortRunJob]
    [MemoryDiagnoser]
    public class CreateBarcodeBenchmark : BenchmarkBase
    {
        private readonly string _text = Guid.NewGuid().ToString();

        [Benchmark]
        public override void Iron_BarCode()
        {
            _ = IronBarCode.QRCodeWriter.CreateQrCode(
                _text,
                500,
                IronBarCode.QRCodeWriter.QrErrorCorrectionLevel.Medium);
        }
    }
}
