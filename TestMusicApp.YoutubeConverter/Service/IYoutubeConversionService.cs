using System.IO;
using System.Threading.Tasks;

namespace TestMusicApp.YoutubeConverter.Service
{
    public interface IYoutubeConversionService
    {
        Task ConvertVideoToMp3Async(string videoId, Stream target);
    }
}
