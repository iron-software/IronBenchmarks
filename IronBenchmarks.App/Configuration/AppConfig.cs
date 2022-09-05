namespace Benchmarks.App.Configuration
{
    public class AppConfig : IAppConfig
    {
        public string? Environment { get; set; }
        public string? LicenseKeyIronXl { get; set; }
        public string? LicenseKeyIronPdf { get; set; }
        public string? LicenseKeyIronBarCode { get; set; }
        public string? LicenseKeyIronOcr { get; set; }
        public string ResultsFolderName { get; set; } = "Results";
    }
}