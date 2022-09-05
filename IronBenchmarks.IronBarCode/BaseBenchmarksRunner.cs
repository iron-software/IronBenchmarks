using IronBenchmarks.Core;
using System.Collections.Generic;

namespace IronBenchmarks.IronBarCode
{
    public abstract class BaseBenchmarksRunner : BenchmarksRunner
    {
        public BaseBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
            BenchmarkMethods = new Dictionary<string, string>()
            {
                { "Generate500QrCodes", "Generate 500 QR codes" },
                { "Generate500QrCodesSaveFiles", "Generate 500 QR codes, saving resilting images" }
            };
        }

        public abstract void Generate500QrCodes();
        public abstract void Generate500QrCodesSaveFiles();
    }
}
