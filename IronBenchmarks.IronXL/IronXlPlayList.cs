using System;
using System.Collections.Generic;
using IronBenchmarks.Core;

namespace IronBenchmarks.IronXL
{
    public class IronXlPlayList : GenericPlayList
    {
        public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultsFolder)
        {
            var curIronXlRunner = new CurrentIronXLBenchmarksRunner(resultsFolder);
            var prevIronXlRunner = new PreviousIronXLBenchmarksRunner(resultsFolder);
            var asposeRunner = new AsposeCellsBenchmarksRunner(resultsFolder);
            var npoiRunner = new NpoiBenchmarksRunner(resultsFolder);
            var closedXmlRunner = new ClosedXmlBenchmarksRunner(resultsFolder);
            //var officeRunner = new OfficeInteropBenchmarkRunner(resultsFolder);

            var result = new Dictionary<string, Dictionary<string, TimeSpan>>()
            {
                { asposeRunner.NameAndVersion, asposeRunner.RunBenchmarks() },
                { curIronXlRunner.NameAndVersion, curIronXlRunner.RunBenchmarks() },
                { prevIronXlRunner.NameAndVersion, prevIronXlRunner.RunBenchmarks() },
                { npoiRunner.NameAndVersion, npoiRunner.RunBenchmarks() },
                { closedXmlRunner.NameAndVersion, closedXmlRunner.RunBenchmarks() },
                //{ officeRunner.NameAndVersion, officeRunner.RunBenchmarks() },
            };

            //officeRunner.QuitExcelApp();

            return result;
        }
    }
}
