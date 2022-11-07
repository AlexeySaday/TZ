using System.Web.Http;

namespace ServiceB
{
    public class ValuesController : ApiController
    { 
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
 
    }
}
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc; 

//namespace ServiceB
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class WeatherController : ControllerBase
//    {
//        [HttpGet]
//        public ActionResult<IEnumerable<OnlyNeedfulForecast>> Get()
//        {
//            return new List<OnlyNeedfulForecast>() { new OnlyNeedfulForecast("jkjk",DateTime.Now)};
//        }
//    }
//}
