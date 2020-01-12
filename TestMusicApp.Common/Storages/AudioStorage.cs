using System.IO;
using System.Threading.Tasks;
using TestMusicApp.Common.Configs;

namespace TestMusicApp.Common.Storages
{
    public class AudioStorage : BaseStorage, IAudioStorage
    {
        private readonly IStorageConfig _storageConfig;

        public AudioStorage(IStorageConfig storageConfig)
            : base(storageConfig)
        {
            this._storageConfig = storageConfig;
        }
        
        public async Task UploadAudioFileAsync(string fileName, Stream content)
        {
            var container = GetContainer(_storageConfig.AudioFilesContainerName);

            await container.CreateIfNotExistsAsync();

            content.Seek(0, SeekOrigin.Begin);

            var blob = container.GetBlockBlobReference(fileName);
            await blob.UploadFromStreamAsync(content);
        }

        public async Task DeleteAudioFileAsync(string fileName)
        {
            var container = GetContainer(_storageConfig.AudioFilesContainerName);

            var blob = container.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }
    }
}
