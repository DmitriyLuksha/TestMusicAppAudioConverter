using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.AudioConverter.Listeners;
using TestMusicApp.AudioConverter.RequestProcessors;
using TestMusicApp.AudioConverter.Services;
using TestMusicApp.AudioConverter.Storages;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.MessageBrokers;
using TestMusicApp.Common.Storages;

namespace TestMusicApp.AudioConverter
{
    public static class ServiceFactory
    {
        private static ServiceProvider _serviceProvider;

        public static void Init(StatelessServiceContext context)
        {
            _serviceProvider = new ServiceCollection()
                .AddSingleton(context)
                .AddSingleton<AudioConverterStatelessService>()
                .AddSingleton<IAudioConversionRequestListener, AudioConversionRequestListener>()
                .AddSingleton<IConversionConfig, ConversionConfig>()
                .AddSingleton<IServiceBusConfig, ServiceBusConfig>()
                .AddSingleton<IStorageConfig, StorageConfig>()
                .AddSingleton<IAudioConversionService, AudioConversionService>()
                .AddSingleton<IAudioUploadingMessageBroker, AudioUploadingMessageBroker>()
                .AddSingleton<IUnprocessedAudioFilesStorage, UnprocessedAudioFilesStorage>()
                .AddSingleton<IAudioConversionRequestProcessor, AudioConversionRequestProcessor>()
                .AddSingleton<IAudioStorage, AudioStorage>()
                .BuildServiceProvider();
        }

        public static TService GetInstance<TService>()
            where TService : class
        {
            return _serviceProvider.GetService<TService>();
        }
    }
}
