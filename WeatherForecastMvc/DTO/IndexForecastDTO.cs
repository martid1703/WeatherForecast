using System.ComponentModel.DataAnnotations;

namespace WeatherForecastMvc.Models
{
    public class IndexForecastDTO
    {
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public uint Span{ get; set; }
        public DayForecastDTO[] Forecast { get; set; }

        public IndexForecastDTO(DateTime date, uint span, DayForecastDTO[] forecast)
        {
            Date = date;
            Span = span;
            Forecast = forecast;
        }
    }
}