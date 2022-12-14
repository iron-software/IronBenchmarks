using BenchmarkDotNet.Attributes;
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
using System.Globalization;
using System.Reflection;

var host = SetupApplication();

ApplyIronXLLicenseKey(host);

var reportConfig = GetReportConfig(args, host);
var reportGenerator = new ReportGenerator(reportConfig);

RunExcelBenchmarks(args, reportConfig, reportGenerator);
RunPdfBenchmarks(args, reportConfig, reportGenerator);
RunBarCodeBenchmarks(args, reportConfig, reportGenerator);

Console.ReadKey();

//#############################################################################

static void RunBarCodeBenchmarks(
    string[] args,
    IReportingConfig reportConfig,
    ReportGenerator reportGenerator)
{
    if (args.Contains("-bc"))
    {
        reportConfig.ReportsFolder += "\\BarCode";

        var barcodeSummaries = new List<Summary>
        {
            BenchmarkRunner.Run<CreateBarcodeBenchmark>()
        };

        reportGenerator.GenerateReport(barcodeSummaries, "BarCodeLibs");
    }
}

static void RunPdfBenchmarks(
    string[] args,
    IReportingConfig reportConfig,
    ReportGenerator reportGenerator)
{
    if (args.Contains("-pdf"))
    {
        reportConfig.ReportsFolder += "\\PDF";

        var config = new ManualConfig()
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

        reportGenerator.GenerateReport(pdfSummaries, "PDFLibs");
    }
}

static void RunExcelBenchmarks(
    string[] args,
    IReportingConfig reportConfig,
    ReportGenerator reportGenerator)
{
    if (args.Contains("-xl"))
    {
        reportConfig.ReportsFolder += "\\Excel";

        var libsWithVersions = GetLibNamesWithVersions(typeof(RandomCellsBenchmark));

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
        };

        reportGenerator.GenerateReport(excelSummaries, "ExcelLibs", libsWithVersions);
    }
}

static Dictionary<string, string> GetLibNamesWithVersions(Type type)
{
    var libNames = new Dictionary<string, string>();
    var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Where(m => m.IsDefined(typeof(BenchmarkAttribute), false));
    var fields = type.BaseType?.GetFields() ?? Array.Empty<FieldInfo>();

    foreach (var property in fields)
    {
        var propertyType = property.FieldType;

        foreach (var method in methods)
        {
            if (property.Name.ToLower().Contains(method.Name.ToLower()))
            {
                var methodName = method.Name;
                var assembly = propertyType.Assembly;
                var assemblyName = assembly.GetName();
                var assemblyVersion = assemblyName.Version;

                if (libNames.ContainsKey(methodName))
                {
                    libNames[methodName] = assemblyVersion?.ToString() ?? "";
                }
                else
                {
                    libNames.Add(methodName, assemblyVersion?.ToString() ?? "");
                }

                break;
            }
        }
    }

    return libNames;
}

static void ApplyIronXLLicenseKey(IHost host)
{
    var appConfig = ActivatorUtilities
        .GetServiceOrCreateInstance<IAppConfig>(host.Services);

    IronXL.License.LicenseKey = appConfig.LicenseKeyIronXl;
}

static IReportingConfig GetReportConfig(string[] args, IHost host)
{
    var reportConfig = ActivatorUtilities
        .GetServiceOrCreateInstance<IReportingConfig>(host.Services);

    if (args.Contains("-a") || args.Contains("-append"))
    {
        reportConfig.AppendToLastReport = true;
    }

    return reportConfig;
}

static IHost SetupApplication()
{
    Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

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
                _ => configurationRoot.GetSection(nameof(AppConfig))
                .Get<AppConfig>() ?? new AppConfig());
            services.AddSingleton<IReportingConfig, ReportingConfig>(
                _ => configurationRoot.GetSection(nameof(ReportingConfig))
                .Get<ReportingConfig>() ?? new ReportingConfig());
        })
        .Build();
    return host;
}