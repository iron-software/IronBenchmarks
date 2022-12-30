using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using BenchmarkDotNet.Validators;
using IronBenchmarks.App.Configuration;
using IronBenchmarks.BarCodeLibs.Benchmarks;
using IronBenchmarks.ExcelLibs.Benchmarks;
using IronBenchmarks.PdfLibs.Benchmarks;
using IronBenchmarks.Reporting;
using IronBenchmarks.Reporting.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;
using static Org.BouncyCastle.Math.EC.ECCurve;

Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.InvariantCulture;

var environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
var builder = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.json", true, true)
    .AddJsonFile($"appsettings.{environment}.json", true, true)
    .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
    .AddEnvironmentVariables();

var configurationRoot = builder.Build();

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddSingleton<IAppConfig, AppConfig>(
            _ => configurationRoot.GetSection(nameof(AppConfig)).Get<AppConfig>() ?? new AppConfig());
        services.AddSingleton<IReportingConfig, ReportingConfig>(
            _ => configurationRoot.GetSection(nameof(ReportingConfig)).Get<ReportingConfig>() ?? new ReportingConfig());
    })
    .Build();

var appConfig = ActivatorUtilities.GetServiceOrCreateInstance<IAppConfig>(host.Services);
IronXL.License.LicenseKey = appConfig.LicenseKeyIronXl;

var reportConfig = ActivatorUtilities.GetServiceOrCreateInstance<IReportingConfig>(host.Services);

if (args.Contains("-a") || args.Contains("-append"))
{
    reportConfig.AppendToLastReport = true;
}

var reportGenerator = new ReportGenerator(reportConfig);

var excelSummaries = new List<Summary>
{
    BenchmarkRunner.Run<RandomCellsBenchmark>(),
    BenchmarkRunner.Run<DateCellBenchmark>(),
    BenchmarkRunner.Run<StyleChangeBenchmark>(),
    BenchmarkRunner.Run<FormulaCellBenchmark>(),
    BenchmarkRunner.Run<LoadLargeFileBenchmark>(),
    BenchmarkRunner.Run<SaveLargeFileBenchmark>(),
    BenchmarkRunner.Run<SortRangeBenchmark>(),
    BenchmarkRunner.Run<AccessingRangePropertiesBenchmark>(),

    //BenchmarkRunner.Run<EmptyBenchmark>(),
    //BenchmarkRunner.Run<OtherEmptyBenchmark>(),
};

reportGenerator.GenerateReport(excelSummaries, "IronXL");

/*var config = new ManualConfig()
        .WithOptions(ConfigOptions.DisableOptimizationsValidator)
        .AddValidator(JitOptimizationsValidator.DontFailOnError)
        .AddLogger(ConsoleLogger.Default)
        .AddColumnProvider(DefaultColumnProviders.Instance);

var pdfSummaries = new List<Summary>
{
    BenchmarkRunner.Run<RenderHtmlToPdfBenchmark>(config),
    BenchmarkRunner.Run<LoadingLargeFileBenchmark>(config),
    BenchmarkRunner.Run<SavingLargeFileBenchmark>(config),
};

reportGenerator.GenerateReport(pdfSummaries, "IronPdf");

var pdfSummaries = new List<Summary>
{
    BenchmarkRunner.Run<CreateBarcodeBenchmark>()
};

reportGenerator.GenerateReport(pdfSummaries, "IronBarCode");*/

Console.ReadKey();