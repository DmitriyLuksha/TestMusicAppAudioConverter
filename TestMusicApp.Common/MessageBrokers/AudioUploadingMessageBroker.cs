using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.Messages;

namespace TestMusicApp.Common.MessageBrokers
{
    public class AudioUploadingMessageBroker : IAudioUploadingMessageBroker
    {
        private readonly IServiceBusConfig _serviceBusConfig;

        public AudioUploadingMessageBroker(IServiceBusConfig serviceBusConfig)
        {
            this._serviceBusConfig = serviceBusConfig;
        }

        public async Task SendFileConversionResult(AudioUploadingResultMessage audioUploadingResultMessage)
        {
            var connectionString = _serviceBusConfig.ConnectionString;
            var queueName = _serviceBusConfig.AudioUploadingResultQueueName;
            
            var messageJson = JsonConvert.SerializeObject(audioUploadingResultMessage);
            var queueClient = new QueueClient(connectionString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson));

            await queueClient.SendAsync(message);

            await queueClient.CloseAsync();
        }
    }
}
