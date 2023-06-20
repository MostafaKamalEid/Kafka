using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Kafka
{
    public class ProduceService : IHostedService
    {
        private Timer? _timer = null;
        private int executionCount = 0;
        private readonly ProducerWrapper _producerWrapper;
        private readonly IServiceProvider _serviceProvider;
        public ProduceService(ProducerWrapper producerWrapper, IServiceProvider serviceProvider)
        {
            _producerWrapper = producerWrapper;
            _serviceProvider = serviceProvider;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {

            _timer = new Timer(async (state) => await DoWork(state), null, TimeSpan.Zero,
                TimeSpan.FromSeconds(3));
            return Task.CompletedTask;
        }

        private async Task DoWork(object state)
        {
            var message = $"message Produced number {executionCount} {DateTime.Now.ToShortTimeString()}";
            Interlocked.Increment(ref executionCount);
            await _producerWrapper.writeMessage(message);
        }




        public Task StopAsync(CancellationToken cancellationToken)
        {

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

    }
}
