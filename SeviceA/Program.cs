using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SeviceA
{
    internal class Program
    {
        static void Main(string[] args)
        { 
            CreateHostBuilder(args).Build().Run();
        }
        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, collection) =>
            {
                collection.AddHostedService<KafkaConsumerHost>();
                collection.AddHostedService<KafkaProducerHost>();
            }); 
    }
    public class KafkaProducerHost : IHostedService
    {
        private readonly ILogger<KafkaProducerHost> _logger;
        private IProducer<Null, WeatherForecast> producer;
        public KafkaProducerHost(ILogger<KafkaProducerHost> logger)
        {
            _logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            producer = new ProducerBuilder<Null, WeatherForecast>(config).Build();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while(true)
            {
                await producer.ProduceAsync(topic: "weather-topic", new Message<Null, WeatherForecast>
                {
                    Value = WeatherData.GetWeatherForecasts()
                }, cancellationToken) ; 
                Thread.Sleep(TimeSpan.FromMinutes(1));
            } 
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            producer?.Dispose();
            return Task.CompletedTask;
        }
    }
    public class KafkaConsumerHost : IHostedService
    {  
        private IConsumer<Null, WeatherForecast> consumer;
        
        public KafkaConsumerHost(ILogger<KafkaProducerHost> logger)
        {
            var config = new ConsumerConfig
            {
                GroupId = "weather-consumer-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Latest, 
            };
            consumer = new ConsumerBuilder<Null, WeatherForecast>(config).Build();
            consumer.Subscribe("weather-topic"); 
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var response = consumer.Consume(cancellationToken);
            if(response.Message != null)
            {
                string weatherDescription = $"На улице {response.Value.weather[0].description}, скорость ветра порядка {response.Value.wind.speed} м/с, температура-{response.Value.main.temp}";
                WeatherData.SetWeather(weatherDescription,DateTime.Now);
            } 
            return Task.CompletedTask;
        } 
        public Task StopAsync(CancellationToken cancellationToken)
        {
            consumer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
 
