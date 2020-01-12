using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using TestMusicApp.Common.Configs;

namespace TestMusicApp.AudioConverter.Storages
{
    public class AudioStorage : IAudioStorage
    {
        private readonly IStorageConfig _storageConfig;

        public AudioStorage(IStorageConfig storageConfig)
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

        private CloudBlobContainer GetContainer(string containerName)
        {
            var account = CloudStorageAccount.Parse(_storageConfig.ConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);

            return container;
        }
    }
}
