using Confluent.Kafka;
using System.Threading.Tasks;
using System;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka
{
    public class ProducerWrapper
    {
        private string _topicName = "test";
        private IProducer<string, string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config)
        {
            config.BrokerAddressFamily = BrokerAddressFamily.V4;
            this._config = config;
            this._producer = new ProducerBuilder<string, string>(this._config).Build();

        }
        public async Task writeMessage(string message)
        {
            var dr = await this._producer.ProduceAsync(this._topicName, new Message<string, string>()
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.WriteLine($"KAFKA => Sent Message '{dr.Value}' to '{dr.TopicPartitionOffset.Offset}'");
            Console.BackgroundColor = ConsoleColor.Green;

            return;
        }
    }
}
