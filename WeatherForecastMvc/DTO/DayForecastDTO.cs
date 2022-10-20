using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecastMvc.Models
{
    public class DayForecastDTO
    {
        public Guid Id { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [BindProperty]
        [DisplayName("Temperature, C\u00ba")]
        [DataType(DataType.Currency, ErrorMessage = "Must be a Decimal")]
        public float[] Temp { get; set; }

        public double AvgTemp
        {
            get
            {
                if (Temp.Length == 0)
                {
                    return 0;
                }
                return Math.Round(Temp.Average(), 2);
            }
        }

        public double MaxTemp
        {
            get
            {
                if (Temp.Length == 0)
                {
                    return 0;
                }
                return Temp.Max();
            }
        }

        public DayForecastDTO()
        {
            Temp = new float[24];
        }
    }
}