using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.Common.DependencyInjection;
using TestMusicApp.YoutubeConverter.DependencyInjection;

namespace TestMusicApp.YoutubeConverter
{
    internal static class Program
    {
        private const string ServiceName = "TestMusicApp.YoutubeConverterType";

        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync(ServiceName, CreateService)
                    .GetAwaiter()
                    .GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id,
                    typeof(YoutubeConverterStatelessService).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static YoutubeConverterStatelessService CreateService(StatelessServiceContext context)
        {
            ConfigureServices(context);
            return ServiceFactory.GetInstance<YoutubeConverterStatelessService>();
        }

        private static void ConfigureServices(StatelessServiceContext context)
        {
            // We can't get it using configs because DI isn't set up yet
            var instrumentationKey = context
                .CodePackageActivationContext
                .GetConfigurationPackageObject("Config")
                .Settings
                .Sections["YoutubeConverter"]
                .Parameters["InstrumentationKey"]
                .Value;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommonServices(context);
            serviceCollection.AddApplicationInsightsTelemetry(instrumentationKey);
            serviceCollection.AddYoutubeConverterServices();

            ServiceFactory.SetupServiceProvider(serviceCollection);
        }
    }
}
