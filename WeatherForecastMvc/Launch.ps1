param ([Parameter(Mandatory=$true)][string] $launchType)

$url="https://localhost:7200"

cd "D:\Progs\Tutorials\WeatherForecastMvc"
Write-Output("Script execution folder: $PSScriptRoot")
$host.UI.RawUI.WindowTitle = "WeatherForecast"

$environment=""
if($launchType -eq "debug")
{
$environment="Development"
dotnet run -c $launchType /p:EnvironmentName=$environment
}
if($launchType -eq "release")
{
$environment="Release"
dotnet build -c release
Start-Process powershell -ArgumentList ".\bin\release\net6.0\WeatherForecastMvc.exe"
}

[System.Diagnostics.Process]::Start($url)
Write-Output "Running $($launchType) version. Environment is $environment."
