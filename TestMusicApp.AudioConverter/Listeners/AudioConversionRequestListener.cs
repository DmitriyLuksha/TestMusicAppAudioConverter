using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.Logging;
using TestMusicApp.AudioConverter.Messages;
using TestMusicApp.AudioConverter.RequestProcessors;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.Listeners;

namespace TestMusicApp.AudioConverter.Listeners
{
    public class AudioConversionRequestListener :
        BaseServiceBusListener<AudioConversionMessage>,
        IAudioConversionRequestListener
    {
        private readonly IAudioConversionRequestProcessor _audioConversionRequestProcessor;

        public AudioConversionRequestListener(
            IServiceBusConfig serviceBusConfig,
            IAudioConversionRequestProcessor audioConversionRequestProcessor,
            ITelemetryChannel telemetryChannel,
            TelemetryClient telemetryClient,
            ILogger<AudioConversionRequestListener> logger
        ) : base(serviceBusConfig,
                telemetryChannel,
                telemetryClient,
                logger,
                serviceBusConfig.AudioConversionQueueName,
                "Audio Converter")
        {
            this._audioConversionRequestProcessor = audioConversionRequestProcessor;
        }

        protected override Task<bool> ProcessServiceBusMessage(AudioConversionMessage message)
        {
            return _audioConversionRequestProcessor.ProcessAsync(message);
        }
    }
}
