using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using TestMusicApp.AudioConverter.Configs;
using TestMusicApp.AudioConverter.Messages;

namespace TestMusicApp.AudioConverter.MessageBrokers
{
    public class AudioConversionMessageBroker : IAudioConversionMessageBroker
    {
        private readonly IServiceBusConfig _serviceBusConfig;

        public AudioConversionMessageBroker(IServiceBusConfig serviceBusConfig)
        {
            this._serviceBusConfig = serviceBusConfig;
        }

        public async Task SendFileConversionResult(AudioConversionResultMessage audioConversionResultMessage)
        {
            var connectionString = _serviceBusConfig.ConnectionString;
            var queueName = _serviceBusConfig.AudioUploadingResultQueueName;
            
            var messageJson = JsonConvert.SerializeObject(audioConversionResultMessage);
            var queueClient = new QueueClient(connectionString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson));

            await queueClient.SendAsync(message);

            await queueClient.CloseAsync();
        }
    }
}
