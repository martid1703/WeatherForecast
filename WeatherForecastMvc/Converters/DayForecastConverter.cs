using WeatherForecastMvc.Models;

public static class DayForecastConverter
{
    public static DayForecastDTO Convert(this DayForecast forecast)
    {
        return new DayForecastDTO { Id = forecast.Id, Date = forecast.Date, Temp = TempConverter.Convert(forecast.Temp) };
    }

    public static DayForecast Convert(this DayForecastDTO forecast)
    {
        return new DayForecast { Id = forecast.Id, Date = forecast.Date, Temp = TempConverter.Convert(forecast.Temp) };
    }

    public static DayForecast Convert(this DayForecastCreateDTO forecast)
    {
        return new DayForecast { Id = Guid.Empty, Date = forecast.Date, Temp = TempConverter.Convert(forecast.Temp) };
    }
}