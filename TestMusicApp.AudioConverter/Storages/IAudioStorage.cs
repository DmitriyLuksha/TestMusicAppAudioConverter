using System.IO;
using System.Threading.Tasks;

namespace TestMusicApp.AudioConverter.Storages
{
    public interface IAudioStorage
    {
        Task ReadUnprocessedAudioFileAsync(string fileName, Stream target);

        Task DeleteUnprocessedAudioFileAsync(string fileName);

        Task UploadAudioFileAsync(string fileName, Stream content);

        Task DeleteAudioFileAsync(string fileName);
    }
}
