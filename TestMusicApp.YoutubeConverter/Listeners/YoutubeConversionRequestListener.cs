using System.Threading.Tasks;
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
            IYoutubeConversionRequestProcessor youtubeConversionRequestProcessor
        ) : base(serviceBusConfig, serviceBusConfig.YoutubeConversionQueueName)
        {
            this._youtubeConversionRequestProcessor = youtubeConversionRequestProcessor;
        }

        protected override async Task ProcessServiceBusMessage(YoutubeConversionMessage message)
        {
            await _youtubeConversionRequestProcessor.ProcessAsync(message);
        }
    }
}
