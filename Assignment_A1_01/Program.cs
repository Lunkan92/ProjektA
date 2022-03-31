using Assignment_A1_01.Services;
using System;
using System.Linq;

namespace Assignment_A1_01
{
    class Program
    {
        static void Main(string[] args)
        {
            double latitude = 59.5086798659495;
            double longitude = 18.2654625932976;
            //double latitude = 59.700;
            //double longitude = 16.933;

            var t1 = new OpenWeatherService().GetForecastAsync(latitude, longitude);
            t1.Wait();
            var forecast = t1.Result;

            //Your Code


            Console.WriteLine($"Weather forecast for {forecast.City}");
            var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);
            foreach (var group in GroupedList)
            {
                Console.WriteLine(group.Key.Date.ToShortDateString());
                foreach (var item in group)
                {
                    Console.WriteLine($"   - {item.DateTime.ToShortTimeString()}: {item.Description}, temperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                }
            }

        }
    }
}

/*
    In class OpenWeatherService you should create an async method, GetForecastAsync(..) that
    implements following functionality:

    -Requests weather data from https://api.openweathermap.org/ using your apiKey.
    -Read the Json response string async.
    - Convert the Json string into the class WeatherApiData.
    -Create a response object of type Forecast from WeatherApiData using Linq.
    - return the Forecast object to the caller.

    The caller, Program.Main should implement following functionality:
    -Call the GetForecastAsync(..) using the asynchronous design methodology.
    - Wait for the task to be completed.
    - Group the Forecast according to Dates using Linq.
    -Write the weather data to the console.

    Hint:
    The timestamp from the weather service is in Unit time stamp format. In one of my
    examples you see how to convert. Linq operator Select is a perfect tool together with
    Lambda Expression to convert one collection to another. Linq operator GroupBy is perfect
    for grouping. Remember how to Wait for all tasks to complete. ReadFromJsonAsync<>()
    extension is an excellent way to get the Json response string.
*/
