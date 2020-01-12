using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Newtonsoft.Json;
using TestMusicApp.Common.Configs;

namespace TestMusicApp.Common.Listeners
{
    public abstract class BaseServiceBusListener<TMessage> : ICommunicationListener
    {
        private readonly IServiceBusConfig _serviceBusConfig;
        private readonly string _queueName;

        private const string DeadLetterExceptionReason = "Can't process request due to exception";

        private IQueueClient _queueClient;

        protected BaseServiceBusListener(
            IServiceBusConfig serviceBusConfig,
            string queueName
        )
        {
            this._serviceBusConfig = serviceBusConfig;
            this._queueName = queueName;
        }

        public Task<string> OpenAsync(CancellationToken cancellationToken)
        {
            var serviceBusConnectionString = _serviceBusConfig.ConnectionString;
            
            _queueClient = new QueueClient(serviceBusConnectionString, this._queueName);

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
                var deserializedMessage = JsonConvert.DeserializeObject<TMessage>(messageJson);

                await this.ProcessServiceBusMessage(deserializedMessage);
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

        protected abstract Task ProcessServiceBusMessage(TMessage message);

        protected virtual void Stop()
        {
            _queueClient
                ?.CloseAsync()
                .GetAwaiter()
                .GetResult();
        }

        protected virtual Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            // TODO: Logging
            return Task.CompletedTask;
        }
    }
}
