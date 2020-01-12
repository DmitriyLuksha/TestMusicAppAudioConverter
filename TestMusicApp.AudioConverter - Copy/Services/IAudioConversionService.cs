using System.IO;

namespace TestMusicApp.AudioConverter.Services
{
    public interface IAudioConversionService
    {
        void ConvertToMp3(Stream content, Stream target);
    }
}
