namespace TestMusicApp.Common.Configs
{
    public interface IServiceBusConfig
    {
        string ConnectionString { get; }

        string AudioConversionQueueName { get; }
        
        string YoutubeConversionQueueName { get; }

        string AudioUploadingResultQueueName { get; }
    }
}
