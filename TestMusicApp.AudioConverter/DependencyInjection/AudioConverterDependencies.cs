using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.AudioConverter.Listeners;
using TestMusicApp.AudioConverter.RequestProcessors;
using TestMusicApp.AudioConverter.Services;
using TestMusicApp.AudioConverter.Storages;

namespace TestMusicApp.AudioConverter
{
    public static class AudioConverterDependencies
    {
        public static void AddAudioConverterServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddSingleton<AudioConverterStatelessService>()
                .AddSingleton<IAudioConversionRequestListener, AudioConversionRequestListener>()
                .AddSingleton<IAudioConversionService, AudioConversionService>()
                .AddSingleton<IUnprocessedAudioFilesStorage, UnprocessedAudioFilesStorage>()
                .AddSingleton<IAudioConversionRequestProcessor, AudioConversionRequestProcessor>()
                .BuildServiceProvider();
        }
    }
}
