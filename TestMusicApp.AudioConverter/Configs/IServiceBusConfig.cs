namespace TestMusicApp.AudioConverter.Configs
{
    public interface IServiceBusConfig
    {
        string ConnectionString { get; }

        string AudioConversionQueueName { get; }

        string AudioUploadingResultQueueName { get; }
    }
}
