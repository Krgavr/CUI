using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using WPF_cviceni2.Models;

namespace WPF_cviceni2.Services 
{
    class WeatherService
    {
        private readonly HttpClient _client;
        private const string API_KEY = "add api key ";
        private const string BASE_URL = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric";
        public WeatherService()
        {
            this._client = new HttpClient();
        }

        public async Task<WeatherModel> GetWeatherInfo(string city)
        {
            string url = string.Format(BASE_URL, city, API_KEY);

            HttpResponseMessage response = await _client.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Failed the request!");
            }

            string jsonResponse = await response.Content.ReadAsStringAsync();

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonResponse));
            JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };


            WeatherModel weather = await JsonSerializer.DeserializeAsync<WeatherModel>(stream, options);

            return weather;
        }

        public string ConvertTime(long datetime, int offset)
        {
            DateTimeOffset utcOffset = DateTimeOffset.FromUnixTimeSeconds(datetime);

            TimeSpan timeSpan = TimeSpan.FromSeconds(offset);
            DateTimeOffset localDateTime = utcOffset.ToOffset(timeSpan);

            string formattedTime = localDateTime.ToString("HH:mm");

            return formattedTime;
        }

    }
}
