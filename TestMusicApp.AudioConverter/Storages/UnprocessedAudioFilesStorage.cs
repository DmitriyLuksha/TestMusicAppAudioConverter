using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using TestMusicApp.Common.Configs;
using TestMusicApp.Common.Storages;

namespace TestMusicApp.AudioConverter.Storages
{
    public class UnprocessedAudioFilesStorage : BaseStorage, IUnprocessedAudioFilesStorage
    {
        private readonly IStorageConfig _storageConfig;

        public UnprocessedAudioFilesStorage(IStorageConfig storageConfig)
            : base(storageConfig)
        {
            this._storageConfig = storageConfig;
        }

        public async Task ReadUnprocessedAudioFileAsync(string fileName, Stream target)
        {
            var container = GetContainer(_storageConfig.UnprocessedAudioFilesContainerName);

            var blob = container.GetBlockBlobReference(fileName);
            
            await blob.DownloadToStreamAsync(target);
        }

        public async Task DeleteUnprocessedAudioFileAsync(string fileName)
        {
            var container = GetContainer(_storageConfig.UnprocessedAudioFilesContainerName);

            var blob = container.GetBlockBlobReference(fileName);
            await blob.DeleteIfExistsAsync();
        }
    }
}
