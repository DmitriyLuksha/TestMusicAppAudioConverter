using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.MessageBrokers;
using TestMusicApp.Common.Storages;
using TestMusicApp.YoutubeConverter.Listeners;
using TestMusicApp.YoutubeConverter.RequestProcessors;
using TestMusicApp.YoutubeConverter.Service;

namespace TestMusicApp.YoutubeConverter.DependencyInjection
{
    public static class YoutubeConverterDependencies
    {
        public static void AddYoutubeConverterServices(this IServiceCollection services)
        {
            services
                .AddSingleton<YoutubeConverterStatelessService>()
                .AddSingleton<IYoutubeConversionRequestListener, YoutubeConversionRequestListener>()
                .AddSingleton<IYoutubeConversionRequestProcessor, YoutubeConversionRequestProcessor>()
                .AddSingleton<IYoutubeConversionService, YoutubeConversionService>();
        }
    }
}
