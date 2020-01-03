using System.Fabric;

namespace TestMusicApp.AudioConverter.Configs
{
    public class StorageConfig : BaseConfig, IStorageConfig
    {
        private const string SectionName = "StorageSettings";

        public StorageConfig(StatelessServiceContext serviceContext)
            : base(serviceContext, SectionName)
        {
        }

        public string ConnectionString =>
            GetParameterValue("ConnectionString");

        public string AudioFilesContainerName =>
            GetParameterValue("AudioFilesContainerName");

        public string UnprocessedAudioFilesContainerName =>
            GetParameterValue("UnprocessedAudioFilesContainerName");
    }
}
