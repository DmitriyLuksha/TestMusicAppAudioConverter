using System.Fabric;

namespace TestMusicApp.Common.Configs
{
    public class ConversionConfig : BaseConfig, IConversionConfig
    {
        private const string SectionName = "ConversionSettings";

        public ConversionConfig(StatelessServiceContext serviceContext)
            : base(serviceContext, "ConversionSettings")
        {
        }

        public int BitRate =>
            int.Parse(GetParameterValue("BitRate"));
    }
}
