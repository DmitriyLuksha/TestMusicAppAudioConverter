using System;
using System.Threading.Tasks;

namespace TestMusicApp.AudioConverter.RequestProcessors
{
    public interface IAudioConversionRequestProcessor
    {
        Task ProcessAsync(string fileName, Guid playlistId, string trackName);
    }
}
