using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.ServiceFabric;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.ApplicationInsights;

namespace TestMusicApp.Common.DependencyInjection
{
    public static class ApplicationInsightsTelemetry
    {
        public static void AddApplicationInsightsTelemetry(this IServiceCollection services, string instrumentationKey)
        {
            var channel = new InMemoryChannel();

            var telemetryConfiguration = TelemetryConfiguration.CreateDefault();

            telemetryConfiguration.InstrumentationKey = instrumentationKey;
            telemetryConfiguration.TelemetryChannel = channel;

            telemetryConfiguration.TelemetryInitializers.Add(new OperationCorrelationTelemetryInitializer());
            telemetryConfiguration.TelemetryInitializers.Add(new FabricTelemetryInitializer());
            telemetryConfiguration.TelemetryInitializers.Add(new HttpDependenciesParsingTelemetryInitializer());

            var telemetryClient = new TelemetryClient(telemetryConfiguration);

            services.AddSingleton<ITelemetryChannel>(channel);
            services.AddSingleton(telemetryConfiguration);
            services.AddSingleton(telemetryClient);

            services.AddLogging(builder =>
            {
                builder.AddApplicationInsights(instrumentationKey);
            });
        }
    }
}
