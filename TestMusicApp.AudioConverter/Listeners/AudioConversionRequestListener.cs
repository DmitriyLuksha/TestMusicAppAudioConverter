using Microsoft.Azure.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
            IAudioConversionRequestProcessor audioConversionRequestProcessor
        ) : base(serviceBusConfig, serviceBusConfig.AudioConversionQueueName)
        {
            this._audioConversionRequestProcessor = audioConversionRequestProcessor;
        }

        protected override async Task ProcessServiceBusMessage(AudioConversionMessage message)
        {
            await _audioConversionRequestProcessor.ProcessAsync(message);
        }
    }
}
