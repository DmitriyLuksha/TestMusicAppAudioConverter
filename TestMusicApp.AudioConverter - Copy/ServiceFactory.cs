using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.AudioConverter.Configs;
using TestMusicApp.AudioConverter.Listeners;
using TestMusicApp.AudioConverter.MessageBrokers;
using TestMusicApp.AudioConverter.RequestProcessors;
using TestMusicApp.AudioConverter.Services;
using TestMusicApp.AudioConverter.Storages;

namespace TestMusicApp.AudioConverter
{
    public static class ServiceFactory
    {
        private static ServiceProvider _serviceProvider;

        public static void Init(StatelessServiceContext context)
        {
            _serviceProvider = new ServiceCollection()
                .AddLogging()
                .AddSingleton(context)
                .AddSingleton<AudioConverterStatelessService>()
                .AddSingleton<IAudioConversionRequestListener, AudioConversionRequestListener>()
                .AddSingleton<IConversionConfig, ConversionConfig>()
                .AddSingleton<IServiceBusConfig, ServiceBusConfig>()
                .AddSingleton<IStorageConfig, StorageConfig>()
                .AddSingleton<IAudioConversionService, AudioConversionService>()
                .AddSingleton<IAudioConversionMessageBroker, AudioConversionMessageBroker>()
                .AddSingleton<IAudioStorage, AudioStorage>()
                .AddSingleton<IAudioConversionRequestProcessor, AudioConversionRequestProcessor>()
                .BuildServiceProvider();
        }

        public static TService GetInstance<TService>()
            where TService : class
        {
            return _serviceProvider.GetService<TService>();
        }
    }
}
