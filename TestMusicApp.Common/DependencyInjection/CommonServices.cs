using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.MessageBrokers;
using TestMusicApp.Common.Storages;

namespace TestMusicApp.Common.DependencyInjection
{
    public static class CommonServices
    {
        public static void AddCommonServices(this IServiceCollection services, ServiceContext context)
        {
            services.AddSingleton(context)
                .AddSingleton<IConversionConfig, ConversionConfig>()
                .AddSingleton<IServiceBusConfig, ServiceBusConfig>()
                .AddSingleton<IStorageConfig, StorageConfig>()
                .AddSingleton<IAudioUploadingMessageBroker, AudioUploadingMessageBroker>()
                .AddSingleton<IAudioStorage, AudioStorage>();

            if (context is StatelessServiceContext statelessServiceContext)
            {
                services.AddSingleton(statelessServiceContext);
            }
        }
    }
}
