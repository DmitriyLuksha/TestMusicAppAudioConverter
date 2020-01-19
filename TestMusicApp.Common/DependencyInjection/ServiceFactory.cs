using System.Fabric;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.MessageBrokers;
using TestMusicApp.Common.Storages;

namespace TestMusicApp.Common.DependencyInjection
{
    public static class ServiceFactory
    {
        private static ServiceProvider _serviceProvider;
        
        public static void SetupServiceProvider(IServiceCollection serviceCollection)
        {
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        public static TService GetInstance<TService>()
            where TService : class
        {
            return _serviceProvider.GetService<TService>();
        }
    }
}
