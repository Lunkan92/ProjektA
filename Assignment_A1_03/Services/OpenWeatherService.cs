using Assignment_A1_03.Models;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

namespace Assignment_A1_03.Services
{
    public delegate void WeatherForecastAvailableHandler(object sender, string message);

    public class OpenWeatherService
    {
        ConcurrentDictionary<(DateTime,string), Forecast> _forecastCacheCity = new ConcurrentDictionary<(DateTime,string), Forecast>();
        ConcurrentDictionary<(DateTime, double, double), Forecast> _forecastCacheLongLat = new ConcurrentDictionary<(DateTime, double, double), Forecast>();
        public event WeatherForecastAvailableHandler WeatherForecastAvailable;
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "01e1d7002da561ca5aca0dac28fbae18"; // Your API Key

        // part of your event and cache code here



        public async Task<Forecast> GetForecastAsync(string City)
        {
            //part of cache code here
            var cities = City;
            var date = DateTime.Now;
            var key = (date, cities);

            if (!_forecastCacheCity.TryGetValue(key, out var forecast))
            {
                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";
                forecast = await ReadWebApiAsync(uri);
                _forecastCacheCity[key] = forecast;
                WeatherForecastAvailable.Invoke(forecast, $"New weather forecast for {City} avalible");

            }
            else if (forecast.Items[0].DateTime.AddMinutes(1) > date)
            {

                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";
                forecast = await ReadWebApiAsync(uri);
                _forecastCacheCity[key] = forecast;
                WeatherForecastAvailable.Invoke(forecast, $"Ne weather forecast for {City} avalible");
            }
            else
            {
                WeatherForecastAvailable.Invoke(forecast, $"Cached weather forecast for {City} avalible");

            }


            ////https://openweathermap.org/current
            //var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            //var uri = $"https://api.openweathermap.org/data/2.5/forecast?q={City}&units=metric&lang={language}&appid={apiKey}";

            //Forecast forecast = await ReadWebApiAsync(uri);
            //WeatherForecastAvailable.Invoke(forecast, $"Cached weather forecast for {City} avalible");
            //part of event and cache code here
            //generate an event with different message if cached data

            return forecast;

        }


        public async Task<Forecast> GetForecastAsync(double latitude, double longitude)
        {
            var longit = longitude;
            var latit = latitude;
            var date = DateTime.Now;
            var key = (date, longit, latit);
            //part of cache code here


            if (!_forecastCacheLongLat.TryGetValue(key, out var forecast))
            {


                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

                forecast = await ReadWebApiAsync(uri);
                _forecastCacheLongLat[key] = forecast;
                WeatherForecastAvailable.Invoke(forecast, $"New weather forecast for ({latitude},{longitude}) avalible");


            }
            else if (forecast.Items[0].DateTime.AddMinutes(1) > date)
            {
                //https://openweathermap.org/current
                var language = System.Globalization.CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
                var uri = $"https://api.openweathermap.org/data/2.5/forecast?lat={latitude}&lon={longitude}&units=metric&lang={language}&appid={apiKey}";

                forecast = await ReadWebApiAsync(uri);
                _forecastCacheLongLat[key] = forecast;
                WeatherForecastAvailable.Invoke(forecast, $"New weather forecast for ({latitude},{longitude}) avalible");
            }
            else
            {
                WeatherForecastAvailable.Invoke(forecast, $"Cached weather forecast for ({latitude},{longitude}) avalible");

            }

            //part of event and cache code here
            //generate an event with different message if cached data

            return forecast;
        }
        private async Task<Forecast> ReadWebApiAsync(string uri)
        {
            // part of your read web api code here
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            WeatherApiData wd = await response.Content.ReadFromJsonAsync<WeatherApiData>();
            // part of your data transformation to Forecast here

            var forecast = new Forecast
            {

                City = wd.city.name,
                Items = wd.list.Select(x => new ForecastItem
                {

                    Temperature = x.main.temp,
                    WindSpeed = x.wind.speed,
                    Description = x.weather[0].description,
                    Icon = x.weather[0].icon,
                    DateTime = UnixTimeStampToDateTime(x.dt)
                }).ToList()
            };
            //generate an event with different message if cached data



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
