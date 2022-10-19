using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace WeatherForecastMvc.Models
{
    public class DayForecastCreateDTO
    {
        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

        [BindProperty]
        [DisplayName("Temperature, C\u00ba")]
        [MaxLength(24, ErrorMessage = "Temperature array cannot be over 24 items")]
        public float[] Temp { get; set; }

        public DayForecastCreateDTO()
        {
            Temp = new float[24];
        }
    }
}