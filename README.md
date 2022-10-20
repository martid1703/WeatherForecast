# WeatherForecast
This is a learning project to create Web service for weather forecast keeping using ASP.NET Core MVC + EF Core + MS SQL + some JS for the frontend logic

## Details:
- can show forecast for specified amount of days
- can add forecast for the next 7 days
- can add forecast for 24h in selected day
- info about old forecast is auto deleted from DB
- can highlight next hottest day from the selected day in calendar on index page

## Defaults:
appsettings.json contains configuration values for the app
{
  "WeatherForecastMvcConfig": {
    "DisplayForecastDays": 30, // how much days to display
    "CalendarSpanDays": 7, // limit amount of selectable days in create forecast mode
    "OldRecordsCleanupDays": 10, // all records prior to Today minus this value will be purged by periodic activity
    "CleanupPeriodSec": 10 // purge activity timeout
  },

## Launch
To launch in debug or release mode via Powershell
- in Powershell execute "cd {your_location}\WeatherForecast\WeatherForecastMvc\Automation\" and then ".\Launch.ps1 debug" or ".\Launch.ps1 release"
