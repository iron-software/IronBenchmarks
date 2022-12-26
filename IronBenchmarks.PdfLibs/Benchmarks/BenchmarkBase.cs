using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    public abstract class BenchmarkBase
    {
        public IronPdf.ChromePdfRenderer IronPdfRenderer;
        public IronPdfOld.ChromePdfRenderer IronPdfOldRenderer;

        public BenchmarkBase()
        {
            SetupLicenses();
        }

        public static void SetupLicenses()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var configuration = builder.Build();
            var appConfig = configuration.GetSection("AppConfig");
            var LicenseKeyIronPdf = appConfig["LicenseKeyIronXl"];

            IronPdf.License.LicenseKey = LicenseKeyIronPdf;
            IronPdfOld.License.LicenseKey = LicenseKeyIronPdf;
        }

        public static void EnsureResultsFolderExists()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var reportsFolder = Path.Combine(path ?? "", "Results");

            if (!Directory.Exists(reportsFolder))
            {
                Directory.CreateDirectory(reportsFolder);
            }
        }

        [IterationSetup]
        public void IterationSetup()
        {
            IronPdfRenderer = new IronPdf.ChromePdfRenderer();
            //IronPdfOldRenderer = new IronPdfOld.ChromePdfRenderer();
        }

        public abstract void Iron_Pdf();

        public abstract void Iron_PdfOld();

        [IterationCleanup]
        public void IterationCleanup()
        {
            IronPdfRenderer = null;
            IronPdfOldRenderer = null;
        }
    }
}