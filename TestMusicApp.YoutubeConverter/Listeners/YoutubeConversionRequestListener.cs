using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.Extensions.Logging;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.Listeners;
using TestMusicApp.YoutubeConverter.Messages;
using TestMusicApp.YoutubeConverter.RequestProcessors;

namespace TestMusicApp.YoutubeConverter.Listeners
{
    public class YoutubeConversionRequestListener :
        BaseServiceBusListener<YoutubeConversionMessage>,
        IYoutubeConversionRequestListener
    {
        private readonly IYoutubeConversionRequestProcessor _youtubeConversionRequestProcessor;

        public YoutubeConversionRequestListener(
            IServiceBusConfig serviceBusConfig,
            IYoutubeConversionRequestProcessor youtubeConversionRequestProcessor,
            ITelemetryChannel telemetryChannel,
            TelemetryClient telemetryClient,
            ILogger<YoutubeConversionRequestListener> logger
        ) : base(serviceBusConfig,
            telemetryChannel,
            telemetryClient,
            logger,
            serviceBusConfig.YoutubeConversionQueueName,
            "YouTube converter")
        {
            this._youtubeConversionRequestProcessor = youtubeConversionRequestProcessor;
        }

        protected override Task<bool> ProcessServiceBusMessage(YoutubeConversionMessage message)
        {
            return _youtubeConversionRequestProcessor.ProcessAsync(message);
        }
    }
}
