using System.Threading.Tasks;
using TestMusicApp.YoutubeConverter.Messages;

namespace TestMusicApp.YoutubeConverter.RequestProcessors
{
    public interface IYoutubeConversionRequestProcessor
    {
        Task ProcessAsync(YoutubeConversionMessage message);
    }
}
