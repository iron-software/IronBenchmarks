using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace IronBenchmarks.Reporting.Tests
{
    public class TestsBase
    {
        public TestsBase()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets(Assembly.GetExecutingAssembly(), true);
            var configuration = builder.Build();
            var appConfig = configuration.GetSection("AppConfig");
            var licenseKeyIronXl = appConfig["LicenseKeyIronXl"];

            IronXL.License.LicenseKey = licenseKeyIronXl;
        }
    }
}
