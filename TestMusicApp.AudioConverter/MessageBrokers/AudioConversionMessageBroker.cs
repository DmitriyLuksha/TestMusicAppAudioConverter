using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using TestMusicApp.AudioConverter.Configs;

namespace TestMusicApp.AudioConverter.MessageBrokers
{
    public class AudioConversionMessageBroker : IAudioConversionMessageBroker
    {
        private readonly IServiceBusConfig _serviceBusConfig;

        public AudioConversionMessageBroker(IServiceBusConfig serviceBusConfig)
        {
            this._serviceBusConfig = serviceBusConfig;
        }

        private readonly string FileConversionResultIsSuccessPropertyKey = "isSuccess";
        private readonly string FileConversionResultUploadedFileNamePropertyKey = "uploadedFileName";
        private readonly string FileConversionResultPlaylistIdPropertyKey = "playlistId";
        private readonly string FileConversionResultTrackNamePropertyKey = "trackName2";

        public async Task SendFileConversionResult(
            bool isSuccess,
            string uploadedFileName,
            Guid playlistId,
            string trackName
        )
        {
            var connectionString = _serviceBusConfig.ConnectionString;
            var queueName = _serviceBusConfig.AudioUploadingResultQueueName;

            var messageDictionary = new Dictionary<string, string>
            {
                { FileConversionResultIsSuccessPropertyKey, isSuccess.ToString() },
                { FileConversionResultUploadedFileNamePropertyKey, uploadedFileName },
                { FileConversionResultPlaylistIdPropertyKey, playlistId.ToString() },
                { FileConversionResultTrackNamePropertyKey, trackName },
            };

            var messageJson = JsonConvert.SerializeObject(messageDictionary);
            var queueClient = new QueueClient(connectionString, queueName);
            var message = new Message(Encoding.UTF8.GetBytes(messageJson));

            await queueClient.SendAsync(message);

            await queueClient.CloseAsync();
        }
    }
}
