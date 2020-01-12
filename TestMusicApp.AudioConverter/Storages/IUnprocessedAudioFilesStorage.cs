using System.IO;
using System.Threading.Tasks;

namespace TestMusicApp.AudioConverter.Storages
{
    public interface IUnprocessedAudioFilesStorage
    {
        Task ReadUnprocessedAudioFileAsync(string fileName, Stream target);

        Task DeleteUnprocessedAudioFileAsync(string fileName);
    }
}
