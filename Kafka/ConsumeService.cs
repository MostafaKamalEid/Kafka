using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static Confluent.Kafka.ConfigPropertyNames;
using static System.Formats.Asn1.AsnWriter;

namespace Kafka
{
    public class ConsumeService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ConsumerWrapper _consumerWrapper;
        private readonly ProducerWrapper _producerWrapper;

        private readonly IConsumer<string, string> _consumer;
        public ConsumeService(ConsumerWrapper consumerWrapper, IServiceProvider serviceProvider, ProducerWrapper producerWrapper)
        {
            _consumerWrapper = consumerWrapper;
            _serviceProvider = serviceProvider;
            _consumer = consumerWrapper._consumer;
            _producerWrapper = producerWrapper;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            Task.Run(async () => await ConsumeMessages(cancellationToken), cancellationToken);


            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        private async Task ConsumeMessages(CancellationToken cancellationToken)
        {



            while (true)
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                if (consumeResult.Message == null)
                {
                    continue;
                }
                Thread t1 = new Thread(async _ => await ProcessMessageAsync(consumeResult));
                t1.Start();

            }
        }
        private async Task ProcessMessageAsync(ConsumeResult<string, string> consumeResult)
        {
            try
            {
                _consumer.Pause(_consumer.Assignment);
                // Delay to simulate processing time for the message 
                var definition = new
                {
                    succeeded = false,
                    secondsCount = 0.0f,
                    proccessingDuration = 0.0f,
                    fileSize = 0.0f,
                    transcript = new[] { new { speaker = "", text = "", start = 0.0f, end = 0.0f } },
                    requestId = "",
                    fileName = "",
                    userId = "",
                    error = "",
                };
              /*  await Task.Delay(1000 * 20);*/
                //Handle the response
                var result = JsonConvert.DeserializeAnonymousType(consumeResult.Value, definition);
                await _producerWrapper.writeMessage(result.userId, result.fileName, result.requestId);
                // Commit the message to the broker
                _consumer.Commit(consumeResult);
                // Resume the partition so that the consumer can consume the next message
                _consumer.Resume(_consumer.Assignment);


                await Console.Out.WriteLineAsync($"Consumed Message  the offset is {consumeResult.Offset} and the value is  {consumeResult.Message.Value}");

            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }

}

