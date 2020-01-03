using System;
using System.Threading.Tasks;

namespace TestMusicApp.AudioConverter.MessageBrokers
{
    public interface IAudioConversionMessageBroker
    {
        Task SendFileConversionResult(bool isSuccess, string uploadedFileName, Guid playlistId, string trackName);
    }
}
