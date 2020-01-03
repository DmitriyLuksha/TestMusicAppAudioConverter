using Microsoft.Azure.ServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestMusicApp.AudioConverter.Configs;
using TestMusicApp.AudioConverter.RequestProcessors;

namespace TestMusicApp.AudioConverter.Listeners
{
    public class AudioConversionRequestListener : IAudioConversionRequestListener
    {
        private readonly IServiceBusConfig _serviceBusConfig;
        private readonly IAudioConversionRequestProcessor _audioConversionRequestProcessor;

        private const string FileNameMessagePropertyKey = "fileName";
        private const string PlaylistIdMessagePropertyKey = "playlistId";
        private const string TrackNameMessagePropertyKey = "trackName";

        private const string DeadLetterExceptionReason = "Can't process request due to exception";

        private IQueueClient _queueClient;

        public AudioConversionRequestListener(
            IServiceBusConfig serviceBusConfig,
            IAudioConversionRequestProcessor audioConversionRequestProcessor
        )
        {
            this._serviceBusConfig = serviceBusConfig;
            this._audioConversionRequestProcessor = audioConversionRequestProcessor;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var serviceBusConnectionString = _serviceBusConfig.ConnectionString;
            var queueName = _serviceBusConfig.AudioConversionQueueName;

            _queueClient = new QueueClient(serviceBusConnectionString, queueName);

            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            _queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
            
            return Task.FromResult(string.Empty);
        }

        public Task CloseAsync(CancellationToken cancellationToken)
        {
            Stop();

            return Task.CompletedTask;
        }

        public void Abort()
        {
            Stop();
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken cancellationToken)
        {
            try
            {
                var messageJson = Encoding.UTF8.GetString(message.Body);
                var messageDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(messageJson);

                var fileName = messageDictionary[FileNameMessagePropertyKey];
                var playlistId = Guid.Parse(messageDictionary[PlaylistIdMessagePropertyKey]);
                var trackName = messageDictionary[TrackNameMessagePropertyKey];

                await _audioConversionRequestProcessor.ProcessAsync(fileName, playlistId, trackName);
            }
            catch (Exception ex)
            {
                // TODO Logging

                var exceptionJson = JsonConvert.SerializeObject(ex);

                await _queueClient.DeadLetterAsync(
                    message.SystemProperties.LockToken,
                    DeadLetterExceptionReason,
                    exceptionJson);

                return;
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private void Stop()
        {
            _queueClient
                ?.CloseAsync()
                .GetAwaiter()
                .GetResult();
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            // TODO: Proper logging
            Console.WriteLine(args.Exception);
            return Task.CompletedTask;
        }
    }
}
