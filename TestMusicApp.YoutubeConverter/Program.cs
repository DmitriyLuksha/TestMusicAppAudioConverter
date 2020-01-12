using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;

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
            ServiceFactory.Init(context);
            return ServiceFactory.GetInstance<YoutubeConverterStatelessService>();
        }
    }
}
