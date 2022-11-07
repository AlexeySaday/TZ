using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceB
{
    public class OnlyNeedfulForecast
    {
        public string fullWeatherForecast { get; set; }
        public DateTime timeOfGet { get; set; }
        public OnlyNeedfulForecast(string fullWeatherForecast, DateTime timeOfGet)
        {
            this.fullWeatherForecast = fullWeatherForecast;
            this.timeOfGet = timeOfGet;
        }
    }
}
