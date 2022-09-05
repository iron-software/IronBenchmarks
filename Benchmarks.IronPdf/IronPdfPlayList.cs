using System;
using System.Collections.Generic;
using Benchmarks.Runner;

namespace Benchmarks.IronPdfBench
{
    public class IronPdfPlayList : GenericPlayList
    {
        public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList(string resultsFolder)
        {
            var curPdfRunner = new CurrentIronPdfBenchmarksRunner(resultsFolder);
            var prevPdfRunner = new PreviousIronPdfBenchmarksRunner(resultsFolder);

            return new Dictionary<string, Dictionary<string, TimeSpan>>()
            {
                { curPdfRunner.NameAndVersion, curPdfRunner.RunBenchmarks() },
                { prevPdfRunner.NameAndVersion, prevPdfRunner.RunBenchmarks() },
            };
        }
    }
}
