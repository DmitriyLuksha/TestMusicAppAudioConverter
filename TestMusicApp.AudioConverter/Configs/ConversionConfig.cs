using System.Fabric;

namespace TestMusicApp.AudioConverter.Configs
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
