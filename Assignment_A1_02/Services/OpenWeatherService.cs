using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_02.Models;

namespace Assignment_A1_02.Services
{
    public delegate void WeatherForecastAvailableHandler(object sender, string message);
    public class OpenWeatherService
    {
        public event WeatherForecastAvailableHandler WeatherForecastAvailable;
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "01e1d7002da561ca5aca0dac28fbae18"; // Your API Key

        //part of your event code here
        public async Task<Forecast> GetForecastAsync(string City)
        {
            //https://openweathermap.org/current
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";

            Forecast forecast = await ReadWebApiAsync(uri);

            WeatherForecastAvailable.Invoke(forecast,$"New weather forecast for {City} avalible");

            return forecast;

        }
        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            //https://openweathermap.org/current
            var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

            Forecast forecast = await ReadWebApiAsync(uri);

            WeatherForecastAvailable.Invoke(forecast, $"New weather forecast for ({latitude},{longitude}) avalible");

            return forecast;
        }
        private async Task<Forecast> ReadWebApiAsync(string uri)
        {
            // part of your read web api code here
            HttpResponseMessage respone = await httpClient.GetAsync(uri);
            respone.EnsureSuccessStatusCode();
            WeatherApiData weatherApi = await respone.Content.ReadFromJsonAsync<WeatherApiData>();

            var forecast = new Forecast
            {

                City = weatherApi.city.name,
                Items = weatherApi.list.Select(x => new ForecastItem
                {

                    Temperature = x.main.temp,
                    WindSpeed = x.wind.speed,
                    Description = x.weather[0].description,
                    Icon = x.weather[0].icon,
                    DateTime = UnixTimeStampToDateTime(x.dt)
                }).ToList()
            };

            // part of your data transformation to Forecast here
            return forecast;
        }
        private DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
    }
}
