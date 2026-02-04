Write-Host "---==[ BEFORE BUILD" $env:project_name $env:APPVEYOR_BUILD_VERSION "SCRIPT ]==---"
dotnet --version
dotnet restore --verbosity normal

Write-Host "---==[ BUILD" $env:project_name $env:APPVEYOR_BUILD_VERSION "SCRIPT ]==---"
dotnet build --verbosity normal

Write-Host "---==[ AFTER BUILD" $env:project_name $env:APPVEYOR_BUILD_VERSION "SCRIPT ]==---"
dotnet pack --no-build /p:PackageVersion=$env:APPVEYOR_BUILD_VERSION --verbosity normal
#nuget push .\bin\release\$env:project_name.$env:APPVEYOR_BUILD_VERSION.nupkg -ApiKey gnsob4ooc2th7pjtxx7yqflr -Source https://ci.appveyor.com/nuget/keithbarrows/api/v2/package
Push-AppveyorArtifact .\bin\Release\$env:project_name.$env:APPVEYOR_BUILD_VERSION.nupkg