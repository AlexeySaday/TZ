using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging; 

namespace ServiceB
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
            });
    }

    public class KafkaConsumerHost : IHostedService
    {
        private IConsumer<Null, string> consumer;

        public KafkaConsumerHost()
        {
            var config = new ConsumerConfig
            {
                GroupId = "topic-weather-group",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            consumer = new ConsumerBuilder<Null, string>(config).Build();
            consumer.Subscribe("topic-weather");
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            var response = consumer.Consume(cancellationToken);
            if (response.Message != null)
            {
                GetConsumedData.SetWeather(response.Value);
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
