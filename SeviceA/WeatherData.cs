using Newtonsoft.Json; 
using System.Text; 

namespace SeviceA
{
    public static class WeatherData
    { 
        static HttpClient _httpClient;
        static List<OnlyNeedfulForecast> forecasts;

        static WeatherData()
        {
            _httpClient = new HttpClient(); 
            forecasts = new List<OnlyNeedfulForecast>(); 
        }
        public static WeatherForecast GetWeatherForecasts()
        {
            string url = @"https://api.openweathermap.org/data/2.5/weather?lat=55.79&lon=49.12&lang=ru&appid=5c6602453fefa2b08e907fde0a5652a3&units=metric";
            string json = _httpClient.GetStringAsync(url).Result;
            
            return JsonConvert.DeserializeObject<WeatherForecast>(json); 
        }
        public static void SetWeather(string fullWeatherDescription, DateTime timeOfGet)
        { 
            for(int i =0; i < forecasts.Count() && i < 10; i++)
            {
                forecasts[i] = forecasts[i + 1];
            }
            forecasts.Add(new OnlyNeedfulForecast(fullWeatherDescription,timeOfGet));
            string url = @"https://somethingweatherapi";

            var r = _httpClient.PostAsync(
                requestUri: url,
                content: new StringContent(
                    JsonConvert.SerializeObject(forecasts),
                    Encoding.UTF8,
                    mediaType: "application/json")
                ).Result;
        }
    }
} 
