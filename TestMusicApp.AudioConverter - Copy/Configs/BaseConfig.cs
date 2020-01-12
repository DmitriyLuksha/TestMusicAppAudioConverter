using System.Fabric;

namespace TestMusicApp.AudioConverter.Configs
{
    public abstract class BaseConfig
    {
        protected StatelessServiceContext ServiceContext { get; }

        private const string PackageName = "Config";

        private readonly string _sectionName;

        protected BaseConfig(StatelessServiceContext serviceContext, string sectionName)
        {
            this.ServiceContext = serviceContext;
            this._sectionName = sectionName;
        }

        protected string GetParameterValue(string parameterName)
        {
            var configPackage = ServiceContext
                .CodePackageActivationContext
                .GetConfigurationPackageObject(PackageName);

            var parameter = configPackage
                .Settings
                .Sections[_sectionName]
                .Parameters[parameterName];

            return parameter.Value;
        }
    }
}
