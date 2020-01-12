using System.Fabric;

namespace TestMusicApp.Common.Configs
{
    public class ServiceBusConfig : BaseConfig, IServiceBusConfig
    {
        private const string SectionName = "ServiceBusSettings";

        public ServiceBusConfig(StatelessServiceContext serviceContext)
            : base(serviceContext, SectionName)
        {
        }

        public string ConnectionString =>
            GetParameterValue("ConnectionString");

        public string AudioConversionQueueName =>
            GetParameterValue("AudioConversionQueueName");

        public string YoutubeConversionQueueName =>
            GetParameterValue("YoutubeConversionQueueName");

        public string AudioUploadingResultQueueName =>
            GetParameterValue("AudioUploadingResultQueueName");
    }
}
