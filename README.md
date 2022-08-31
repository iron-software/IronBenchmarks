#Benchmarks
A simple framework to measure performance of pretty much anything. After mesurements it creates a report Excel file with a timetable and a chart.

### How to use the repository
  1. Clone the repository to your machine
  2. Add your license key to ..\Benchmarks\Benchmarks.App\appsettings.json file under the "LicenceKeyIronXl" property, removing "PLACE YOUR KEY HERE" placeholder.
  3. Add your license key to ..\Benchmarks\Benchmarks.App\appsettings.json file under the "LicenceKeyIronBarCode" property, removing "PLACE YOUR KEY HERE" placeholder.
  4. Repeat 2 and 3 for ..\Benchmarks\Benchmarks.App.Tests\appsettings.json file if you plan to run any tests.
  5. Check versions of tested nuget packages used in the repository, update or downgrade to your taste/needs.
  6. Run the app
  7. Look for a report under ..\Benchmarks\Benchmarks.App\bin\Debug\net6.0\Reports (path is controlled with "ReportsFolder" property in appsettings.json). Every app run will create new report
  8. Look for saved results of benchmark work under ..\Benchmarks\Benchmarks.App\bin\Debug\net6.0\Results (path is controlled with "ResultsFolderName" property in appsettings.json). Files will be re-written on each app run.

### How to 

### Notes
  * "PreviousIxlBenchmarkRunner is using the customized assembly of older version of IronXL. It was renamed to IronXLOld.dll, all of it's types' namespaces were renamed from IronXL to IronXLOld, and it is stored in an ..\XLBenchmarks\packages folder. It is added to XLBenchmarks project as an assembly, not a nuget package. To use another version of IronXL as IronXLOld perform similar renaming procedure and replace ..\XLBenchmarks\packages\IronXLOld.dll with your version.
  * Benchmarks are pretty slow, so be prepared for the app to run for several minutes, especially if you are benchmarking old IronXL library.
  * To control number of benchmark runners that are ran during benchmarking process - comment/uncomment dictionary entries in..\Benchmarks\Benchmarks.App\Reporting\ReportGenerator.cs file in the method GetTimeTableData().
  * To run Office Interop benchmarks you will need Office installed on your computer.
