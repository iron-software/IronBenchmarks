using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace Benchmarks.Runner
{
    public abstract class BenchmarksRunner
    {
        protected static string resultsFolderName = "Results";
        protected Dictionary<string, string> benchmarkMethods;

        protected abstract string BenchmarkRunnerName { get; }
        public abstract string NameAndVersion { get; }


        public BenchmarksRunner(string resultsFolder)
        {
            resultsFolderName = resultsFolder;
        }

        public Dictionary<string, TimeSpan> RunBenchmarks()
        {
            CreateResultsFolder();

            var timeTable = new Dictionary<string, TimeSpan>();

            FillTimeTable(timeTable);

            return timeTable;
        }

        private void FillTimeTable(Dictionary<string, TimeSpan> timeTable)
        {
            try
            {
                var i = 0;

                foreach (var benchmark in benchmarkMethods)
                {
                    var benchmarkAction = GetAction(benchmark.Key);
                    timeTable.Add(benchmark.Value, RunBenchmark(benchmarkAction));

                    i++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private Action GetAction(string benchmark)
        {
            var type = GetType();
            var method = type.GetMethod(benchmark);
            var action = (Action)method.CreateDelegate(typeof(Action), this);

            return action;
        }

        private static TimeSpan RunBenchmark(Action benchmarkName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            benchmarkName();

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        protected static TimeSpan GetTimeSpan()
        {
            return TimeSpan.FromSeconds(10);
        }

        protected static string GetAssemblyVersion(Type type)
        {
            var assembly = Assembly.GetAssembly(type);
            var assemblyVersion = assembly?.GetName().Version;
            var versionString = assemblyVersion == null ? "unknown" : assemblyVersion.ToString();

            return versionString;
        }

        private static void CreateResultsFolder()
        {
            if (!Directory.Exists(resultsFolderName))
            {
                Directory.CreateDirectory(resultsFolderName);
            }
        }
    }
}
