using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Newtonsoft.Json;
using TestMusicApp.Common.Configs;

namespace TestMusicApp.Common.Listeners
{
    public abstract class BaseServiceBusListener<TMessage> : ICommunicationListener
    {
        private readonly IServiceBusConfig _serviceBusConfig;
        private readonly ITelemetryChannel _telemetryChannel;
        private readonly TelemetryClient _telemetryClient;
        private readonly ILogger _logger;

        private readonly string _queueName;
        private readonly string _serviceName;

        private const string DeadLetterExceptionReason = "Can't process request due to exception";

        private IQueueClient _queueClient;

        protected BaseServiceBusListener(
            IServiceBusConfig serviceBusConfig,
            ITelemetryChannel telemetryChannel,
            TelemetryClient telemetryClient,
            ILogger logger,
            string queueName,
            string serviceName
        )
        {
            this._serviceBusConfig = serviceBusConfig;
            this._telemetryChannel = telemetryChannel;
            this._telemetryClient = telemetryClient;
            this._logger = logger;
            
            this._queueName = queueName;
            this._serviceName = serviceName;
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
                using (_logger.BeginScope($"{_serviceName} - Request processing"))
                {
                    var messageJson = Encoding.UTF8.GetString(message.Body);
                    var deserializedMessage = JsonConvert.DeserializeObject<TMessage>(messageJson);

                    var timer = Stopwatch.StartNew();

                    var isSuccess = await ProcessServiceBusMessage(deserializedMessage);

                    timer.Stop();
                    var elapsedTime = timer.Elapsed;

                    _telemetryClient.TrackRequest(new RequestTelemetry
                    {
                        Name = $"{this._serviceName} - Request completed",
                        Duration = elapsedTime,
                        Success = isSuccess
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Request processing");

                var exceptionJson = JsonConvert.SerializeObject(ex);

                await _queueClient.DeadLetterAsync(
                    message.SystemProperties.LockToken,
                    DeadLetterExceptionReason,
                    exceptionJson);

                return;
            }

            await _queueClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        protected abstract Task<bool> ProcessServiceBusMessage(TMessage message);

        protected virtual void Stop()
        {
            this._telemetryChannel.Flush();

            _queueClient
                ?.CloseAsync()
                .GetAwaiter()
                .GetResult();

            // https://docs.microsoft.com/en-us/azure/azure-monitor/app/api-custom-events-metrics#flushing-data
            Thread.Sleep(5000);
        }

        protected virtual Task ExceptionReceivedHandler(ExceptionReceivedEventArgs args)
        {
            _logger.LogError(args.Exception, "ExceptionReceivedHandler triggered");
            return Task.CompletedTask;
        }
    }
}
