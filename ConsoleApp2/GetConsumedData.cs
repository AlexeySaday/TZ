namespace ServiceB
{
    public static class GetConsumedData
    {
        public static List<OnlyNeedfulForecast> Forecasts;
        static GetConsumedData()
        {
            Forecasts = new List<OnlyNeedfulForecast>();
        }
        public static void SetWeather(string weatherDescription)
        {
            for (int i = 0; i < Forecasts.Count() && i < 10; i++)
            {
                Forecasts[i] = Forecasts[i + 1];
            }
            Forecasts.Add(new OnlyNeedfulForecast(weatherDescription,DateTime.Now));
        }
    }
}
