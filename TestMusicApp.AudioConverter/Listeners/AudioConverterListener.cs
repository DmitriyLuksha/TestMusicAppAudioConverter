using Microsoft.Azure.ServiceBus;
using System;
using System.Threading;
using System.Threading.Tasks;
using TestMusicApp.AudioConverter.Interfaces.Services;

namespace TestMusicApp.AudioConverter.Listeners
{
    public class AudioConverterListener : IAudioConverterListener
    {
        private readonly IApplicationConfig _applicationConfig;
        private IQueueClient _queueClient;

        public AudioConverterListener(IApplicationConfig applicationConfig)
        {
            this._applicationConfig = applicationConfig;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var serviceBusConnectionString = _applicationConfig.ServiceBusConnectionString;
            var queueName = _applicationConfig.AudioConversionQueueName;

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
