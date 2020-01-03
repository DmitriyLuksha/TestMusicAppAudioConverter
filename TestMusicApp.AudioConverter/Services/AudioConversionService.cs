using System.IO;
using NAudio.Lame;
using NAudio.Wave;
using TestMusicApp.AudioConverter.Configs;

namespace TestMusicApp.AudioConverter.Services
{
    public class AudioConversionService : IAudioConversionService
    {
        private readonly IConversionConfig _conversionConfig;

        public AudioConversionService(IConversionConfig conversionConfig)
        {
            this._conversionConfig = conversionConfig;
        }
        
        public void ConvertToMp3(Stream content, Stream target)
        {
            var bitRate = _conversionConfig.BitRate;

            using (var reader = new StreamMediaFoundationReader(content))
            using (var writer = new LameMP3FileWriter(target, reader.WaveFormat, bitRate))
            {
                reader.CopyTo(writer);
            }
        }
    }
}
