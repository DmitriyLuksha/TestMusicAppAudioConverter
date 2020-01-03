namespace TestMusicApp.AudioConverter.Configs
{
    public interface IStorageConfig
    {
        string ConnectionString { get; }

        string AudioFilesContainerName { get; }

        string UnprocessedAudioFilesContainerName { get; }
    }
}
