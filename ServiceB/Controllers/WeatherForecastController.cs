using Microsoft.AspNetCore.Mvc;

namespace WebApplication2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {  
        private readonly ILogger<WeatherForecastController> _logger; 
        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        } 
        [HttpGet]
        public IEnumerable<OnlyNeedfulForecast> Get()
        {
            _logger.LogInformation("Информация передана");
            return GetConsumedData.Forecasts.ToArray();
        }
    }
}