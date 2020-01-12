namespace TestMusicApp.Common.Configs
{
    public interface IConversionConfig
    {
        int BitRate { get; }

        string TempFilesFolder { get; }
    }
}
