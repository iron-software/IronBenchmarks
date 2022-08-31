using System;
using System.Collections.Generic;
using Benchmarks.Runner;

namespace Benchmarks.IronBarCode
{
    public class BarCodePlayList : GenericPlayList
    {
        public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultsFolder)
        {
            var curBarcodeRunner = new CurrentBarCodeBenchmarkRunner(resultsFolder);
            var prevBarcodeRunner = new PreviousBarCodeBenchmarkRunner(resultsFolder);

            return new Dictionary<string, Dictionary<string, TimeSpan>>()
            {
                { curBarcodeRunner.NameAndVersion, curBarcodeRunner.RunBenchmarks() },
                { prevBarcodeRunner.NameAndVersion, prevBarcodeRunner.RunBenchmarks() },
            };
        }
    }
}
