using IronBenchmarks.IronBarCode;
using IronBenchmarks.IronPdf;
using IronBenchmarks.IronXL;
using IronBenchmarks.Reporting;
using IronBenchmarks.App.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;
using IronBenchmarks.Reporting.Configuration;

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
            _ => configurationRoot.GetSection(nameof(AppConfig)).Get<AppConfig>());
        services.AddSingleton<IReportingConfig, ReportingConfig>(
            _ => configurationRoot.GetSection(nameof(ReportingConfig)).Get<ReportingConfig>());
    })
    .UseSerilog()
    .Build();

var appConfig = ActivatorUtilities.GetServiceOrCreateInstance<IAppConfig>(host.Services);
IronXL.License.LicenseKey = appConfig.LicenseKeyIronXl;
IronBarCode.License.LicenseKey = appConfig.LicenseKeyIronBarCode;
IronPdf.License.LicenseKey = appConfig.LicenseKeyIronPdf;

var reportConfig = ActivatorUtilities.GetServiceOrCreateInstance<IReportingConfig>(host.Services);
var reportGenerator = new ReportGenerator(reportConfig);

//var timeTableData = new IronPdfPlayList().RunPlayList(appConfig.ResultsFolderName);
//reportGenerator.GenerateReport(timeTableData, "IronPdf");

var timeTableData = new IronXlPlayList(new Dictionary<string, string>() { { "IronXL", appConfig.LicenseKeyIronXl } }).RunPlayList(appConfig.ResultsFolderName);
reportGenerator.GenerateReport(timeTableData, "IronXL");

//var timeTableData = new IronBarCodePlayList().RunPlayList(appConfig.ResultsFolderName);
//reportGenerator.GenerateReport(timeTableData, "IronBarCode");