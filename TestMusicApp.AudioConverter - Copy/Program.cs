using Microsoft.Extensions.Configuration;
using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Fabric;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using TestMusicApp.AudioConverter.Services;

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
            ServiceFactory.Init(context);
            return ServiceFactory.GetInstance<AudioConverterStatelessService>();
        }
    }
}
