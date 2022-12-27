using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace IronBenchmarks.PdfLibs.Benchmarks
{
    public abstract class BenchmarkBase
    {
        public BenchmarkBase()
        {
            SetupLicenses();
            EnsureResultsFolderExists();
        }

        public static void SetupLicenses()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var configuration = builder.Build();
            var appConfig = configuration.GetSection("AppConfig");
            var LicenseKeyIronPdf = appConfig["LicenseKeyIronPdf"];

            IronPdf.License.LicenseKey = LicenseKeyIronPdf;
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

        public abstract void Iron_Pdf();

        public abstract void Pdf_Sharp();

        public abstract void ITextSharp();
    }
}