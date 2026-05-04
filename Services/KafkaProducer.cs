using AnimalClinic.Contracts;
using Confluent.Kafka;
using System.Text.Json;

namespace AnimalClinic.Services
{
    public class KafkaProducer
    {
        private readonly IProducer<Null, string> _producer;
        private readonly IConfiguration _config;

        public KafkaProducer(IConfiguration config)
        {
            _config = config;

            var producerConfig = new ProducerConfig
            {
                BootstrapServers = _config["Kafka:BootstrapServers"]
            };

            _producer = new ProducerBuilder<Null, string>(producerConfig)
                .Build();
        }

        public async Task PublishAsync(AnimalCreatedEvent message)
        {
            var json = JsonSerializer.Serialize(message);

            await _producer.ProduceAsync(
                _config["Kafka:Topic"],
                new Message<Null, string>
                {
                    Value = json
                });

            Console.WriteLine($"Kafka published: {json}");
        }
    }
}
