using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using TestMusicApp.Common.Configs;

namespace TestMusicApp.Common.Storages
{
    public class BaseStorage
    {
        private readonly IStorageConfig _storageConfig;

        protected BaseStorage(IStorageConfig storageConfig)
        {
            this._storageConfig = storageConfig;
        }

        protected CloudBlobContainer GetContainer(string containerName)
        {
            var account = CloudStorageAccount.Parse(_storageConfig.ConnectionString);
            var serviceClient = account.CreateCloudBlobClient();
            var container = serviceClient.GetContainerReference(containerName);

            return container;
        }
    }
}
