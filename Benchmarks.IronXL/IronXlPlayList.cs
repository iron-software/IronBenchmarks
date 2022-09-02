using System;
using System.Collections.Generic;
using Benchmarks.Runner;

namespace Benchmarks.IronXL
{
    public class IronXlPlayList : GenericPlayList
    {
        public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultsFolder)
        {
            var curIronXlRunner = new CurrentIronXLBenchmarkRunner(resultsFolder);
            var prevIronXlRunner = new PreviousIronXLBenchmarkRunner(resultsFolder);
            var asposeRunner = new AsposeBenchmarkRunner(resultsFolder);
            var npoiRunner = new NpoiBenchmarkRunner(resultsFolder);
            var closedXmlRunner = new ClosedXmlBenchmarkRunner(resultsFolder);
            //var officeRunner = new OfficeInteropBenchmarkRunner(resultsFolder);

            var result = new Dictionary<string, Dictionary<string, TimeSpan>>()
            {
                { curIronXlRunner.NameAndVersion, curIronXlRunner.RunBenchmarks() },
                { prevIronXlRunner.NameAndVersion, prevIronXlRunner.RunBenchmarks() },
                { asposeRunner.NameAndVersion, asposeRunner.RunBenchmarks() },
                { npoiRunner.NameAndVersion, npoiRunner.RunBenchmarks() },
                { closedXmlRunner.NameAndVersion, closedXmlRunner.RunBenchmarks() },
                //{ officeRunner.NameAndVersion, officeRunner.RunBenchmarks() },
            };

            //officeRunner.QuitExcelApp();

            return result;
        }
    }
}
