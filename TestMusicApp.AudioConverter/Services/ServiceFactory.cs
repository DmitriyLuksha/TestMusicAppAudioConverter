using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.AudioConverter.Interfaces.Services;
using TestMusicApp.AudioConverter.Listeners;

namespace TestMusicApp.AudioConverter.Services
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
                .AddSingleton<IAudioConverterListener, AudioConverterListener>()
                .AddSingleton<IApplicationConfig, ApplicationConfig>()
                .BuildServiceProvider();
        }

        public static TService GetInstance<TService>() where TService : class
        {
            return _serviceProvider.GetService<TService>();
        }
    }
}
