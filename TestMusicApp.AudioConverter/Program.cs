using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TestMusicApp.AudioConverter.Services;
using TestMusicApp.Common.DependencyInjection;

namespace TestMusicApp.AudioConverter
{
    internal static class Program
    {
        private const string ServiceName = "TestMusicApp.AudioConverterType";

        private static void Main()
        {
            try
            {
                ServiceRuntime.RegisterServiceAsync(ServiceName, CreateService)
                    .GetAwaiter()
                    .GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id,
                    typeof(AudioConverterStatelessService).Name);

                // Prevents this host process from terminating so services keeps running. 
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }

        private static AudioConverterStatelessService CreateService(StatelessServiceContext context)
        {
            ConfigureServices(context);
            return ServiceFactory.GetInstance<AudioConverterStatelessService>();
        }

        private static void ConfigureServices(StatelessServiceContext context)
        {
            // We can't get it using configs because DI isn't set up yet
            var instrumentationKey = context
                .CodePackageActivationContext
                .GetConfigurationPackageObject("Config")
                .Settings
                .Sections["AudioConverter"]
                .Parameters["InstrumentationKey"]
                .Value;

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddCommonServices(context);
            serviceCollection.AddApplicationInsightsTelemetry(instrumentationKey);
            serviceCollection.AddAudioConverterServices();

            ServiceFactory.SetupServiceProvider(serviceCollection);
        }
    }
}
