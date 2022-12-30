using Microsoft.Extensions.Configuration;
using OfficeOpenXml;
using System.IO;
using System.Reflection;

namespace IronBenchmarks.ExcelLibs.Benchmarks.Bases
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
            var licenseKeyIronXl = appConfig["LicenseKeyIronXl"];

            IronXL.License.LicenseKey = licenseKeyIronXl;
            IronXLOld.License.LicenseKey = licenseKeyIronXl;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
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

        public abstract void IronXl();

        public abstract void Iron_XlOld();

        public abstract void Aspose();

        public abstract void Npoi();

        public abstract void ClosedXml();

        public abstract void Epplus();
    }
}