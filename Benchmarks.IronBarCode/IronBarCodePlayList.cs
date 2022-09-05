using System;
using System.Collections.Generic;
using Benchmarks.Runner;

namespace Benchmarks.IronBarCode
{
    public class IronBarCodePlayList : GenericPlayList
    {
        public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultsFolder)
        {
            var curBarcodeRunner = new CurrentIronBarCodeBenchmarksRunner(resultsFolder);
            var prevBarcodeRunner = new PreviousIronBarCodeBenchmarksRunner(resultsFolder);

            return new Dictionary<string, Dictionary<string, TimeSpan>>()
            {
                { curBarcodeRunner.NameAndVersion, curBarcodeRunner.RunBenchmarks() },
                { prevBarcodeRunner.NameAndVersion, prevBarcodeRunner.RunBenchmarks() },
            };
        }
    }
}
