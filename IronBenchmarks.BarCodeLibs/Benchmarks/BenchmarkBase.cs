using Microsoft.Extensions.Configuration;
using System.IO;
using System.Reflection;

namespace IronBenchmarks.BarCodeLibs.Benchmarks
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
            string licenseKeyIronBarCode = appConfig["LicenseKeyIronBarCode"];

            IronBarCode.License.LicenseKey = licenseKeyIronBarCode;
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

        public abstract void Iron_BarCode();
    }
}
