using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TestMusicApp.AudioConverter.Listeners;

namespace TestMusicApp.AudioConverter
{
    public class AudioConverterStatelessService : StatelessService
    {
        private readonly IAudioConversionRequestListener _audioConversionRequestListener;

        public AudioConverterStatelessService(
            StatelessServiceContext serviceContext,
            IAudioConversionRequestListener audioConversionRequestListener
        ) : base(serviceContext)
        {
            this._audioConversionRequestListener = audioConversionRequestListener;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>
            {
                new ServiceInstanceListener(context => _audioConversionRequestListener)
            };
        }
    }
}
