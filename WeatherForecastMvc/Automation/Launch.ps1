param ([Parameter(Mandatory=$true)][string] $launchType)

cd ..

$url="https://localhost:7200"
[System.Diagnostics.Process]::Start($url)

Write-Output("Script execution folder: $PSScriptRoot")
$host.UI.RawUI.WindowTitle = "WeatherForecast"

$environment=""
if($launchType -eq "debug")
{
$environment="Development"
dotnet run -c $launchType /p:EnvironmentName="Development"
}
if($launchType -eq "release")
{
dotnet run -c $launchType /p:EnvironmentName="Release"
#Start-Process powershell -ArgumentList "-NoExit .\Run.ps1"
}


Write-Output "Running $($launchType) version. Environment is $environment."
