namespace TestMusicApp.Common.Configs
{
    public interface IStorageConfig
    {
        string ConnectionString { get; }

        string AudioFilesContainerName { get; }

        string UnprocessedAudioFilesContainerName { get; }
    }
}
