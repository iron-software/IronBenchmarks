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
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            IConfigurationRoot configuration = builder.Build();
            IConfigurationSection appConfig = configuration.GetSection("AppConfig");
            string licenseKeyIronXl = appConfig["LicenseKeyIronXl"];

            IronXL.License.LicenseKey = licenseKeyIronXl;
            IronXLOld.License.LicenseKey = licenseKeyIronXl;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static void EnsureResultsFolderExists()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string reportsFolder = Path.Combine(path ?? "", "Results");

            if (!Directory.Exists(reportsFolder))
            {
                _ = Directory.CreateDirectory(reportsFolder);
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