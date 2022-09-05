using Benchmarks.Runner;
using System.Collections.Generic;

namespace Benchmarks.IronBarCode
{
    public abstract class IronBarCodeBenchmarksRunner : BenchmarksRunner
    {
        public IronBarCodeBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
            benchmarkMethods = new Dictionary<string, string>()
            {
                { "Generate500QrCodes", "Generate 500 QR codes" },
                { "Generate500QrCodesSaveFiles", "Generate 500 QR codes, saving resilting images" }
            };
        }

        public abstract void Generate500QrCodes();
        public abstract void Generate500QrCodesSaveFiles();
    }
}
