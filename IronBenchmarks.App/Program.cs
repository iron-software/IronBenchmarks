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

IHost host = SetupApplication();

ApplyIronXLLicenseKey(host);

IReportingConfig reportConfig = GetReportConfig(args, host);
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
    if (!args.Contains("-bc"))
    {
        return;
    }

    reportConfig.ReportsFolder += "\\BarCode";

    var barcodeSummaries = new List<Summary>
        {
            BenchmarkRunner.Run<CreateBarcodeBenchmark>()
        };

    _ = reportGenerator.GenerateReport(barcodeSummaries, "BarCodeLibs");
}

static void RunPdfBenchmarks(
    string[] args,
    IReportingConfig reportConfig,
    ReportGenerator reportGenerator)
{
    if (!args.Contains("-pdf"))
    {
        return;
    }

    reportConfig.ReportsFolder += "\\PDF";

    ManualConfig config = new ManualConfig()
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

    _ = reportGenerator.GenerateReport(pdfSummaries, "PDFLibs");
}

static void RunExcelBenchmarks(
    string[] args,
    IReportingConfig reportConfig,
    ReportGenerator reportGenerator)
{
    if (!args.Contains("-xl"))
    {
        return;
    }

    reportConfig.ReportsFolder += "\\Excel";

    Dictionary<string, string> libsWithVersions = GetLibNamesWithVersions(typeof(RandomCellsBenchmark));

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
            BenchmarkRunner.Run<RemoveRowBenchmark>(),
        };

    _ = reportGenerator.GenerateReport(excelSummaries, "ExcelLibs", libsWithVersions);
}

static Dictionary<string, string> GetLibNamesWithVersions(Type type)
{
    var libNames = new Dictionary<string, string>();
    IEnumerable<MethodInfo> methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .Where(m => m.IsDefined(typeof(BenchmarkAttribute), false));
    FieldInfo[] fields = type.BaseType?.GetFields() ?? Array.Empty<FieldInfo>();

    foreach (FieldInfo property in fields)
    {
        Type propertyType = property.FieldType;

        foreach (MethodInfo? method in methods)
        {
            if (property.Name.ToLower().Contains(method.Name.ToLower()))
            {
                string methodName = method.Name;
                Assembly assembly = propertyType.Assembly;
                AssemblyName assemblyName = assembly.GetName();
                Version? assemblyVersion = assemblyName.Version;

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
    IAppConfig appConfig = ActivatorUtilities
        .GetServiceOrCreateInstance<IAppConfig>(host.Services);

    IronXL.License.LicenseKey = appConfig.LicenseKeyIronXl;
}

static IReportingConfig GetReportConfig(string[] args, IHost host)
{
    IReportingConfig reportConfig = ActivatorUtilities
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

    string? environment = Environment.GetEnvironmentVariable("NETCORE_ENVIRONMENT");
    IConfigurationBuilder builder = new ConfigurationBuilder()
        .AddJsonFile($"appsettings.json", true, true)
        .AddJsonFile($"appsettings.{environment}.json", true, true)
        .AddUserSecrets(Assembly.GetExecutingAssembly(), true)
        .AddEnvironmentVariables();

    IConfigurationRoot configurationRoot = builder.Build();

    IHost host = Host.CreateDefaultBuilder()
        .ConfigureServices((context, services) =>
        {
            _ = services.AddSingleton<IAppConfig, AppConfig>(
                _ => configurationRoot.GetSection(nameof(AppConfig))
                .Get<AppConfig>() ?? new AppConfig());
            _ = services.AddSingleton<IReportingConfig, ReportingConfig>(
                _ => configurationRoot.GetSection(nameof(ReportingConfig))
                .Get<ReportingConfig>() ?? new ReportingConfig());
        })
        .Build();
    return host;
}