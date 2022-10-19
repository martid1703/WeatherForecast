Get-Process|Where{$_.MainWindowTitle -eq "WeatherForecast"}|Stop-Process -Force
Get-Process -Name "WeatherForecast*"|Stop-Process -Force