cd \Source Code\QuickFrameMVC\QuickFrame.Data\src\QuickFrame.Data
dotnet pack -c Release
dotnet pack -c Debug
cd \Source Code\QuickFrameMVC\QuickFrame.IO\src\QuickFrame.IO
dotnet pack -c Release
dotnet pack -c Debug
cd \Source Code\QuickFrameMVC\QuickFrame.Security\src\QuickFrame.Security
dotnet pack -c Release
dotnet pack -c Debug
cd \Source Code\QuickFrameMVC\QuickFrame.Security.ActiveDirectory\src\QuickFrame.Security.ActiveDirectory
dotnet pack -c Release
dotnet pack -c Debug
cd \Source Code\QuickFrameMVC\QuickFrame.Mvc\src\QuickFrame.Mvc
dotnet pack -c Release
dotnet pack -c Debug
cd ..\..\..\