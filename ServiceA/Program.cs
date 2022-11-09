using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 

namespace ServiceA
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
                collection.AddHostedService<KafkaProducerHost>();
            });
    }
    public class KafkaProducerHost : IHostedService
    {
        private readonly ILogger<KafkaProducerHost> logger;
        private IProducer<Null, string> producer;
        public KafkaProducerHost(ILogger<KafkaProducerHost> logger)
        {
            this.logger = logger;
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            producer = new ProducerBuilder<Null, string>(config).Build();
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                await producer.ProduceAsync("topic-weather", new Message<Null, string>
                {
                    Value = $"{WeatherData.GetWeatherForecasts().fullWeatherForecast}\n{WeatherData.GetWeatherForecasts().timeOfGet}"
                }, cancellationToken);
                logger.LogInformation("Сообщение отправлено");
                Thread.Sleep(TimeSpan.FromMinutes(1));
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            producer?.Dispose();
            return Task.CompletedTask;
        }
    }
}
