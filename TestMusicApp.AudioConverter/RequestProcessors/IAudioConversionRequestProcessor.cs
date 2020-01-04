using System;
using System.Threading.Tasks;
using TestMusicApp.AudioConverter.Messages;

namespace TestMusicApp.AudioConverter.RequestProcessors
{
    public interface IAudioConversionRequestProcessor
    {
        Task ProcessAsync(AudioConversionMessage message);
    }
}
