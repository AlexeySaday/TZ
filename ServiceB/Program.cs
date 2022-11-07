using Confluent.Kafka; 

namespace WebApplication2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddControllers(); 
            builder.Services.AddHostedService<KafkaConsumerHost>();
            var app = builder.Build(); 
            app.MapControllers(); 
            app.Run();
        } 
    }   
    public class KafkaConsumerHost : IHostedService
    {
        private IConsumer<Null, string> consumer; 
        private readonly ILogger<KafkaConsumerHost> logger;
        private GetConsumedData data;
        public KafkaConsumerHost(ILogger<KafkaConsumerHost> logger)
        {
            data = new();
            var config = new ConsumerConfig
            {
                GroupId = "topic-weather-fifth",
                BootstrapServers = "localhost:9092",
                AutoOffsetReset = AutoOffsetReset.Earliest,
            };
            consumer = new ConsumerBuilder<Null, string>(config).Build();
            consumer.Subscribe("topic-weather");
            this.logger = logger; 
        }
        public Task StartAsync(CancellationToken cancellationToken)
        { 
            Task.Run(() =>
            {
                while (true)
                {
                    var response = consumer.Consume(cancellationToken);
                    if (response != null)
                    {
                        data.SetWeather((OnlyNeedfulForecast)response.Message.Value);
                        logger.LogInformation($"Сообщение в консьюмере получено: {response.Message.Value}"); 
                    }
                }
            }); 
            return Task.CompletedTask;
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {  
            consumer?.Dispose();
            return Task.CompletedTask;
        }
    }
}

