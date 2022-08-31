using Benchmarks.Configuration;
using Benchmarks.IronBarCode;

namespace Benchmarks.App.PlayLists
{
    internal class BarCodePlayList : GenericPlayList
    {
        public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(IAppConfig appConfig)
        {
            var resultsFolder = appConfig.ResultsFolderName;

            var curBarcodeRunner = new CurrentBarCodeBenchmarkRunner(resultsFolder);
            var prevBarcodeRunner = new PreviousBarCodeBenchmarkRunner(resultsFolder);

            return new()
            {
                { curBarcodeRunner.NameAndVersion, curBarcodeRunner.RunBenchmarks() },
                { prevBarcodeRunner.NameAndVersion, prevBarcodeRunner.RunBenchmarks() },
            };
        }
    }
}
