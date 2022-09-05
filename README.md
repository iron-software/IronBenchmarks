# Benchmarks
A simple framework to measure performance of pretty much anything. After measurements it creates a report Excel file with a timetable and a chart.

### How to use the repository
  1. Clone the repository to your machine.
  2. Add your license key to *..\IronBenchmarks\IronBenchmarks.App\appsettings.json* file under the *LicenseKeyIronXl* property, removing "PLACE YOUR KEY HERE" placeholder.
  3. Add your license key to *..\IronBenchmarks\IronBenchmarks.App\appsettings.json* file under the *LicenseKeyIronBarCode* property, removing "PLACE YOUR KEY HERE" placeholder.
  4. Add your license key to *..\IronBenchmarks\IronBenchmarks.App\appsettings.json* file under the *LicenseKeyIronPdf* property, removing "PLACE YOUR KEY HERE" placeholder.
  5. Add your license key to *..\IronBenchmarks\IronBenchmarks.App\appsettings.json* file under the *LicenseKeyIronOcr* property, removing "PLACE YOUR KEY HERE" placeholder.
  6. Check versions of Nuget packages used in the repository that are going to be benchmarked, update or downgrade to your taste/needs.
  7. Run the app.
  8. Look for a complete report under *..\IronBenchmarks\IronBenchmarks.App\bin\Debug\net6.0\Reports* (path is controlled with *ReportsFolder* property in *appsettings.json*). Every app-run will create a new report.
  9. Look for saved results of benchmark work under *..\IronBenchmarks\IronBenchmarks.App\bin\Debug\net6.0\Results* (path is controlled with *ResultsFolderName* property in *appsettings.json*). Files will be re-written on each app run.

### How to create a new benchmarks play list
 1. Create a new project called **MyBenchmarks** in the solution.
 2. Add **IronBenchmarks.Runner** project as a reference to your new project.
 3. Add refrences to Nuget packages/assemblys/projects that you plan to run benchmarks on
 4. Create a heir to IronBenchmarks.Runner.**BenchmarksRunner** abstract class and call it **BaseBenchmarksRunner**. NOTE: Heir itself should be abstract, as benchmarks runners for each Nuget package/assembly/project you are comparing are going to be inheriting from it.
 5. In your **BaseBenchmarksRunner** add public abstract methods, which - when implemented in heir classes - will contain operations that you are willing to measure the performance of. Each of these methods will be a column in the timetable of the resulting report.
 6. In the constructor for your **BaseBenchmarksRunner** you need to initialize the *BenchmarkMethods* *<string, string>* dictionary with a dictionary that contains names of the methods from **step 5**** above as *keys* and their descriptions (which will be used as timetable column headers in the resulting report) as *values*. An example of a very basic **BaseBenchmarksRunner**: 
```csharp
public abstract class BaseBenchmarksRunner : BenchmarksRunner
{
  public BaseBenchmarksRunner(string resultsFolder) : base(resultsFolder)
  {
    BenchmarkMethods = new Dictionary<string, string>()
      {
        { "Generate500QrCodes", "Generate 500 QR codes" },
        { "Generate500QrCodesSaveFiles", "Generate 500 QR codes, save images" }
      };
  }
        
  public abstract void Generate500QrCodes();
  public abstract void Generate500QrCodesSaveFiles();
}
```

 7. Implement BenchmarksRunners that are inherited from your BaseBenchmarksRunner class for each Nuget package/assembly/project you are comparing.
 8. Create a **MyPlayList** class, that inherits from IronBenchmarks.Runner.**GenericPlayList**.
 9. In the override for the *RunPlayList* method create instances of your benchmarks runners from **step 7**, initialize *<string, Dictionary<string, TimeSpan>>* dictionary which will contain a name and a version of the Nuget package/assembly/project you are comparing as *keys* and a *<string, TimeSpan>* dictionary that will contain benchmarks descriptions and times that took a Nuget package/assembly/project to complete those as *values*. Return the resulting dictionary as method output. An example of a very basic *RunPlayList*:
```csharp
public override Dictionary<string, Dictionary<string, TimeSpan>> RunPlayList
  (string resultsFolder)
{
  var curBarcodeRunner = new CurrentIronBarCodeBenchmarksRunner(resultsFolder);
  var prevBarcodeRunner = new PreviousIronBarCodeBenchmarksRunner(resultsFolder);

  return new Dictionary<string, Dictionary<string, TimeSpan>>()
    {
      { curBarcodeRunner.NameAndVersion, curBarcodeRunner.RunBenchmarks() },
      { prevBarcodeRunner.NameAndVersion, prevBarcodeRunner.RunBenchmarks() },
    };
}
```
	
 10. Add your **MyBenchmarks** project as a reference to the **IronBenchmarks.App** project.
 11. In the Program.cs add these lines at the end (or where appropriate):
```csharp
var timeTableData = new MyPlayList().RunPlayList(appConfig.ResultsFolderName);
reportGenerator.GenerateReport(timeTableData, "MyBenchmarks");
```
	
 12. List item "MyBenchmarks" in the *reportGenerator.GenerateReport* call is the prefix that will be added to the report's file name after the app-run.
 13. Make sure that *Benchmarks.App* project is selected as a startup project.
 14. Run the debug.
 15. Check *Results* and *Reports* folders for files created by the app.

### Notes
  * **PreviousIronXlBenchmarksRunner** is using the customized assembly of older version of *IronXL*. It was renamed to *IronXLOld.dll*, all of it's types' namespaces were renamed from IronXL to **IronXLOld**, and it is stored in an *..\IronBenchmarks\packages* folder. It is added to **IronBenchmarks.IronXL** project as an assembly, not a nuget package. To use another version of IronXL as IronXLOld perform similar renaming procedure and replace *..\IronBenchmarks\packages\IronXLOld.dll* with your version.
  * Benchmarks in **IronBenchmarks.IronXL** are pretty slow, so be prepared for the app to run for several minutes, especially if you are benchmarking *Office Interop*.
  * To run Office Interop benchmarks you will need Office installed on your computer.