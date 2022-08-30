using Benchmarks.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Benchmarks.BenchmarkRunners
{
    internal abstract class BenchmarkRunner
    {
        protected readonly IAppConfig _appConfig;
        protected static string _resultsFolderName = "Results";

        public BenchmarkRunner(IAppConfig appConfig)
        {
            _appConfig = appConfig;
            _resultsFolderName = _appConfig.ResultsFolderName;
        }

        public TimeSpan[] RunBenchmarks()
        {
            CreateResultsFolder();

            var numberOfTests = _appConfig.BenchmarkList?.Length ?? 0;
            var timeTable = new TimeSpan[numberOfTests];

            if (numberOfTests > 0)
            {
                FillTimeTable(timeTable);
            }

            return timeTable;
        }

        private void FillTimeTable(TimeSpan[] timeTable)
        {
            try
            {
                // put in elapsed times from benchmarks that were ran in the order in which they come
                // in appsettings.json property BenchmarkList
                timeTable[0] = RunBenchmark(Generate500QrCodes, saveResults: false);
                timeTable[1] = RunBenchmark(Generate500QrCodes, saveResults: true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static TimeSpan RunBenchmark(Action<bool> benchmarkName, bool saveResults)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            benchmarkName(saveResults);

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        protected abstract string BenchmarkRunnerName { get; }
        public abstract string NameAndVersion { get; }
        protected abstract void Generate500QrCodes(bool saveResults);

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
            if (!Directory.Exists(_resultsFolderName))
            {
                Directory.CreateDirectory(_resultsFolderName);
            }
        }
    }
}
