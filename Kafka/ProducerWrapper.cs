using Confluent.Kafka;
using System.Threading.Tasks;
using System;
using static Confluent.Kafka.ConfigPropertyNames;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace Kafka
{
    public class ProducerWrapper
    {
        private string _topicName = "transcriptionOut";
        private IProducer<string, string> _producer;
        private ProducerConfig _config;
        private static readonly Random rand = new Random();

        public ProducerWrapper(ProducerConfig config)
        {
            config.BrokerAddressFamily = BrokerAddressFamily.V4;
/*            config.ReceiveMessageMaxBytes = 1024 * 1024 * 15;
*/            config.MessageMaxBytes = 1024 * 1024 * 15;
            this._config = config;
            this._producer = new ProducerBuilder<string, string>(this._config).Build();
            
        }
        public async Task writeMessage(string userId,string fileName, string requestId)
        {

            var SuccessBody = new
            {
                succeeded = true,
                secondsCount = 50,
                proccessingDuration = 20,
                fileSize = 202452,
                transcript = new[]
                   {
                    new { speaker = "Speaker-1", text = "اذكر اربع رئساء لجمهورية مصر العربية", start = 1.25, end = 5.24 },
                    new { speaker = "Speaker-1", text = "اذكر اربع رئساء لجمهورية مصر العربية", start = 1.25, end = 5.24 },
                    new { speaker = "Speaker-1", text = "اذكر اربع رئساء لجمهورية مصر العربية", start = 1.25, end = 5.24 },
             
                },

                requestId = requestId,
                fileName = fileName,
                userId = userId,
            };

            var dr = await this._producer.ProduceAsync(this._topicName, new Message<string, string>()
            {
                Key = Guid.NewGuid().ToString(),
                Value = JsonConvert.SerializeObject(SuccessBody)
            });
            _producer.Flush();
            return;
        }
    }
}
