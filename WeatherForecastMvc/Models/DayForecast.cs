namespace WeatherForecastMvc.Models
{
    public class DayForecast
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public string Temp { get; set; }
    }
}