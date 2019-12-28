using System.Fabric;
using TestMusicApp.AudioConverter.Interfaces.Services;

namespace TestMusicApp.AudioConverter.Services
{
    public class ApplicationConfig : IApplicationConfig
    {
        private readonly StatelessServiceContext _serviceContext;

        private const string PackageName = "Config";
        private const string ServiceBusSettingsSectionName = "ServiceBusSettings";

        public ApplicationConfig(StatelessServiceContext serviceContext)
        {
            _serviceContext = serviceContext;
        }

        public string ServiceBusConnectionString => GetParameterValue("ServiceBusConnectionString");

        public string AudioConversionQueueName => GetParameterValue("AudioConversionQueueName");

        private string GetParameterValue(string parameterKey)
        {
            var configPackage = _serviceContext
                .CodePackageActivationContext
                .GetConfigurationPackageObject(PackageName);

            var parameter = configPackage
                .Settings
                .Sections[ServiceBusSettingsSectionName]
                .Parameters[parameterKey];

            return parameter.Value;
        }
    }
}
