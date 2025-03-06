using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WPF_cviceni2.Models
{
    public class WeatherModel
    {
        public WeatherMain main { get; set; }
        [JsonPropertyName("dt")]
        public long datetime { get; set; }
        public WeatherSys sys { get; set; }
        [JsonPropertyName("timezone")]
        public int timezoneOffset { get; set; }
        public string name { get; set; }
    }

    public class WeatherMain
    {
        public float temp { get; set; }
        [JsonPropertyName("feels_like")]
        public float feelsLike { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class WeatherSys
    {
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }
}
