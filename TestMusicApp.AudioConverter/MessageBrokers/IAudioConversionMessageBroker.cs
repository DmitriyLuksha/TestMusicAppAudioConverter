using System.Threading.Tasks;
using TestMusicApp.AudioConverter.Messages;

namespace TestMusicApp.AudioConverter.MessageBrokers
{
    public interface IAudioConversionMessageBroker
    {
        Task SendFileConversionResult(AudioConversionResultMessage audioConversionResultMessage);
    }
}
