using System;
using System.Collections.Generic;
using IronBenchmarks.Core;

namespace IronBenchmarks.IronPdf
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
