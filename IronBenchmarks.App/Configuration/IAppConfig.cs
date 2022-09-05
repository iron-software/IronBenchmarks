namespace Benchmarks.App.Configuration
{
    public interface IAppConfig
    {
        string? Environment { get; set; }
        string? LicenseKeyIronXl { get; set; }
        string? LicenseKeyIronPdf { get; set; }
        string? LicenseKeyIronBarCode { get; set; }
        string ResultsFolderName { get; set; }
    }
}