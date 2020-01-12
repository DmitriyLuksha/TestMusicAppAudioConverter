using System.Threading.Tasks;
using TestMusicApp.Common.Messages;

namespace TestMusicApp.Common.MessageBrokers
{
    public interface IAudioUploadingMessageBroker
    {
        Task SendFileConversionResult(AudioUploadingResultMessage audioUploadingResultMessage);
    }
}
