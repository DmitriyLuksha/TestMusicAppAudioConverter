using System.Collections.Generic;
using System.Fabric;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TestMusicApp.AudioConverter.Listeners;

namespace TestMusicApp.AudioConverter
{
    public class AudioConverterStatelessService : StatelessService
    {
        private readonly IAudioConverterListener _audioConverterListener;

        public AudioConverterStatelessService(
            StatelessServiceContext serviceContext,
            IAudioConverterListener audioConverterListener
        ) : base(serviceContext)
        {
            _audioConverterListener = audioConverterListener;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>
            {
                new ServiceInstanceListener(context => _audioConverterListener)
            };
        }
    }
}
