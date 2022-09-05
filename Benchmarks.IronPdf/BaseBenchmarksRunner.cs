﻿using Benchmarks.Runner;
using System.Collections.Generic;

namespace Benchmarks.IronPdfBench
{
    public abstract class BaseBenchmarksRunner : BenchmarksRunner
    {
        public BaseBenchmarksRunner(string resultsFolder) : base(resultsFolder)
        {
            BenchmarkMethods = new Dictionary<string, string>()
            {
                { "Generate10Pdf", "Generate 10 PDFs" },
                { "Generate10PdfSaveFiles", "Generate 10 PDFs, saving resilting files" }
            };
        }

        public abstract void Generate10Pdf();
        public abstract void Generate10PdfSaveFiles();
    }
}