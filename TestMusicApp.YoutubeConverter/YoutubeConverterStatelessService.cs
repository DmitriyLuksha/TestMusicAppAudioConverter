using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using TestMusicApp.YoutubeConverter.Listeners;

namespace TestMusicApp.YoutubeConverter
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class YoutubeConverterStatelessService : StatelessService
    {
        private readonly IYoutubeConversionRequestListener _youtubeConversionRequestListener;

        public YoutubeConverterStatelessService(
            StatelessServiceContext serviceContext,
            IYoutubeConversionRequestListener youtubeConversionRequestListener
        ) : base(serviceContext)
        {
            this._youtubeConversionRequestListener = youtubeConversionRequestListener;
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new List<ServiceInstanceListener>
            {
                new ServiceInstanceListener(context => _youtubeConversionRequestListener)
            };
        }
    }
}
