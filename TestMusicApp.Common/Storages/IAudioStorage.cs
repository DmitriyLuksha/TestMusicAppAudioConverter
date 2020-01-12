using System.IO;
using System.Threading.Tasks;

namespace TestMusicApp.Common.Storages
{
    public interface IAudioStorage
    {
        Task UploadAudioFileAsync(string fileName, Stream content);

        Task DeleteAudioFileAsync(string fileName);
    }
}
