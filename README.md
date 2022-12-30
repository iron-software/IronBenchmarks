# IronBenchmarks
A tool to measure performance of pretty much anything. After measurements it creates an Excel file report with a data on performance and memory allocation presented in charts.

### How to use the repository
 1. Clone the repository to your machine.
 2. This application uses IronXL to create reports based on the data collected from benchmarking various actions. For IronXL to be operational you need to provide it with a License key. To do so add your license key to **..\IronBenchmarks\IronBenchmarks.App\appsettings.json** file under the **LicenseKeyIronXl** property, removing "PLACE YOUR KEY HERE" placeholder.
 3. To use pre-defined project for benchmarking BarCode, PDF or Excel libraries provide license keys for IronBarCode, IronPdf or IronXL. To do so in Visual Studio right-click respective project, then click "Manage User Secrets". In the file secrets.json add one of the following snippets:
 ```json
 {
   "AppConfig": {
     "LicenseKeyIronBarCode": "YOUR_KEY_HERE"
   }
 }
 ```
 ```json
 {
   "AppConfig": {
     "LicenseKeyIronPdf": "YOUR_KEY_HERE"
   }
 }
 ```
 ```json
 {
   "AppConfig": {
     "LicenseKeyIronXl": "YOUR_KEY_HERE"
   }
 }
 ```
 4. Check versions of NuGet packages used in the repository that are going to be benchmarked, update or downgrade to your taste/needs.
 5. Select "Release" configuration.
 6. Build the solution.
 7. Run the app from command line under **..\IronBenchmarks\IronBenchmarks.App\bin\Debug\net6.0\** folder.
  - To run Excel benchmarks use "-xl" command-line argument
  - To run PDF benchmarks use "-pdf" command-line argument
  - To run BarCode benchmarks use "-bc" command-line argument
 8. Look for a complete report under **..\IronBenchmarks\IronBenchmarks.App\bin\Debug\net6.0\Reports** (path is controlled with **ReportsFolder** property in **appsettings.json**). Every app-run will create a new report.
 9. Look for saved results of benchmark work under **..\IronBenchmarks\IronBenchmarks.App\bin\Debug\net6.0\Results** (path is controlled with **ResultsFolderName** property in **appsettings.json**). Files will be re-written on each app run.

### How to create new benchmarks
IronBenchmarks uses [BenchmarkDotNet](https://benchmarkdotnet.org/) for measurements. To learn more about designing benchmarks with it checkout the [Articles section](https://benchmarkdotnet.org/articles/overview.html) on their website.
Things to keep in mind:
 1. Reporting engine of IronBenchmarks is designed in such a way, that any class with a set of benchmarks is a class that measures one particular action by different libraries. For example, **IronBenchmarks.ExcelLibs.Benchmarks.DateCellBenchmark** class will measure how Aspose.Cells, IronXL, ClosedXML, EPPlus and NPOI will perform setting a date to a cell in a pre-created workbook. This design assumes, that each such class will contain equal number of methods marked with _Benchmark_ attribute. Names of methods should reflect the name of the library, that will perform benchmarked action in this method. The names of methods should be the same throughout the classes. To achieve that: it is recommended to use abstract class from which the benchmark classes will inherit the structure of methods. For example checkout the **IronBenchmarks.PdfLibs.Benchmarks.BenchmarkBase** abstract class and how it is used in **IronBenchmarks.PdfLibs.Benchmarks.SavingLargeFileBenchmark** class.
 2. In benchmark class override pre-defined in a base class methods and decorate them with _Benchmark_ attribute.
 3. To tell _BenchmarkDotNet_ that it needs to gather memory allocation info, decorate your class with _MemoryDiagnoser_ attribute.
 4. Decorate benchmark class with _ShortRunJob_ attribute, if you need the runs for the methods of this class to be shorter, which will hinder the precision of the data, but will speed up the execution.
 5. Create methods with _IterationSetup_ and _IterationCleanup_ to setup things before each call of the _Benchmark_ marked method of your class and to cleanup things after it was called. These methods will execute before and after EACH execution of the benchmark methods. For example of how it is used checkout **IronBenchmarks.ExcelLibs.Benchmarks.Bases.SheetOperationsBenchmarkBase** class.
 6. In **IronBenchmarks.App.Program** class add your class to the list of classes that will be run like so:
```csharp
...
var barcodeSummaries = new List<Summary>
{
    BenchmarkRunner.Run<CreateBarcodeBenchmark>(),
    **BenchmarkRunner.Run<MyNewBenchmark>(),**
};
...
```
 7. Repeat steps from **How to use the repository** to see the results of the benchmarking of your new class.

### Useful features
 - The version 0.0.2 of IronBenchmarks allows to append the results of current benchmarking to the report created earlier. This is useful if you want to add new contenders to already created report or (which is more important) to **benchmark different versions of the same library**. For example, You can run benchmark for BarCode with IronBarCode v2022.11, downgrade in the IronBenchmarks.BarCodeLibs IronBarCode v2022.9, build solution again, and run IronBenchmarks.App.exe with a "-a" or "-append" command-line argument. This will run your benchmarks on an older version, but will append the results to the previous report, so you don't have to do it manually.
 - You can tell ReportGenerator to add versions of libraries to the report. To do so checkout implementation of **IronBenchmarks.App.Program.GetLibNamesWithVersions** or pass the ReportGenerator a manually created Dictionary<string, string> where keys are the names of your benchmark contenders (!!!should be the same as the names of your benchmark methods!!!) and values are versions that you wish to see in the report. This will improve in future versions.

### Notes
  * **IronBenchmarks.ExcelLibs** is using the customized assembly of older version of *IronXL*. It was renamed to *IronXLOld.dll*, all of it's types' namespaces were renamed from IronXL to **IronXLOld**, and it is stored in an *..\IronBenchmarks\packages* folder. It is added to **IronBenchmarks.IronXL** project as an assembly, not a NuGet package. To use another version of IronXL as IronXLOld perform similar renaming procedure and replace *..\IronBenchmarks\packages\IronXLOld.dll* with your version.
  * Benchmarking process is really slow, so be prepared for the app to run for several minutes.
  * If you plan to commit any changes to this public repository, please be sure to not include any sensitive information in the commit, such as License keys and so on. To add License keys in a discreet manner, please use User secrets feature of Visual Studio.