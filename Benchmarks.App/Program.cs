using Benchmarks.ReportsEngine.Configuration;
using Benchmarks.IronBarCode;
using Benchmarks.IronXL;
using Benchmarks.ReportsEngine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.Reflection;
using Benchmarks.IronPdfBench;

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
    })
    .UseSerilog()
    .Build();

var appConfig = ActivatorUtilities.GetServiceOrCreateInstance<IAppConfig>(host.Services);
IronXL.License.LicenseKey = appConfig.LicenseKeyIronXl;
IronBarCode.License.LicenseKey = appConfig.LicenseKeyIronBarCode;
IronPdf.License.LicenseKey = appConfig.LicenseKeyIronPdf;

var reportGenerator = new ReportGenerator(appConfig);

var timeTableData = new IronPdfPlayList().RunPlayList(appConfig.ResultsFolderName);
reportGenerator.GenerateReport(timeTableData, "IronPdf");

//timeTableData = new IronXlPlayList().RunPlayList(appConfig.ResultsFolderName);
//reportGenerator.GenerateReport(timeTableData, "IronXL");

//timeTableData = new BarCodePlayList().RunPlayList(appConfig.ResultsFolderName);
//reportGenerator.GenerateReport(timeTableData, "BarCode");