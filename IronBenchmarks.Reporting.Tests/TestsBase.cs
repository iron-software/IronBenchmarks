using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace IronBenchmarks.Reporting.Tests
{
    public class TestsBase
    {
        public TestsBase()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            IConfigurationRoot configuration = builder.Build();
            IConfigurationSection appConfig = configuration.GetSection("AppConfig");
            string? licenseKeyIronXl = appConfig["LicenseKeyIronXl"];

            IronXL.License.LicenseKey = licenseKeyIronXl;
        }
    }
}
