using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.Common.Configs;
using TestMusicApp.YoutubeConverter.Listeners;
using TestMusicApp.YoutubeConverter.RequestProcessors;

namespace TestMusicApp.YoutubeConverter
{
    public static class ServiceFactory
    {
        private static ServiceProvider _serviceProvider;

        public static void Init(StatelessServiceContext context)
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton(context)
                .AddSingleton<YoutubeConverterStatelessService>()
                .AddSingleton<IYoutubeConversionRequestListener, YoutubeConversionRequestListener>()
                .AddSingleton<IYoutubeConversionRequestProcessor, YoutubeConversionRequestProcessor>()
                .AddSingleton<IConversionConfig, ConversionConfig>()
                .AddSingleton<IServiceBusConfig, ServiceBusConfig>()
                .AddSingleton<IStorageConfig, StorageConfig>()
                .BuildServiceProvider();
        }

        public static TService GetInstance<TService>()
            where TService : class
        {
            return _serviceProvider.GetService<TService>();
        }
    }
}
