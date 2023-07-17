using Confluent.Kafka;
using System;
using System.Threading;
using static Confluent.Kafka.ConfigPropertyNames;

namespace Kafka
{

    public class ConsumerWrapper
    {
        private string _topicName = "transcription_topic";
        private ConsumerConfig _consumerConfig;
        public IConsumer<string, string> _consumer;
        public ConsumerWrapper(ConsumerConfig config)
        {
            config.BrokerAddressFamily = BrokerAddressFamily.V4;
            config.AutoOffsetReset = AutoOffsetReset.Earliest;
            config.EnableAutoCommit = false;
            config.EnableAutoOffsetStore = true;
            config.SessionTimeoutMs = 6000;
            config.MaxPollIntervalMs = 6000;
            config.ClientId = "test";
            _consumerConfig = config;
            //Create Consumer with topicename 
            var consumerBuilder = new ConsumerBuilder<string, string>(this._consumerConfig);
            _consumer = consumerBuilder.Build();
            _consumer.Subscribe(_topicName);
        }
        public ConsumeResult<string, string> readMessage()
        {
            var consumeResult = this._consumer.Consume();

            return consumeResult;
        }
        public void Dispose()
        {
            _consumer.Close();
            this.Dispose();
        }
    }
}
