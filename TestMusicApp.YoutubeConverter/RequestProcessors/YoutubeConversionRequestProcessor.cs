using System;
using System.IO;
using System.Threading.Tasks;
using TestMusicApp.Common.Helpers;
using TestMusicApp.Common.MessageBrokers;
using TestMusicApp.Common.Messages;
using TestMusicApp.Common.Storages;
using TestMusicApp.YoutubeConverter.Messages;
using TestMusicApp.YoutubeConverter.Service;

namespace TestMusicApp.YoutubeConverter.RequestProcessors
{
    public class YoutubeConversionRequestProcessor : IYoutubeConversionRequestProcessor
    {
        private readonly IYoutubeConversionService _youtubeConversionService;
        private readonly IAudioStorage _audioStorage;
        private readonly IAudioUploadingMessageBroker _audioUploadingMessageBroker;

        public YoutubeConversionRequestProcessor(
            IYoutubeConversionService youtubeConversionService,
            IAudioStorage audioStorage,
            IAudioUploadingMessageBroker audioUploadingMessageBroker
        )
        {
            this._youtubeConversionService = youtubeConversionService;
            this._audioStorage = audioStorage;
            this._audioUploadingMessageBroker = audioUploadingMessageBroker;
        }

        public async Task ProcessAsync(YoutubeConversionMessage message)
        {
            var stream = new MemoryStream();

            try
            {
                await _youtubeConversionService.ConvertVideoToMp3Async(message.VideoId, stream);
            }
            catch (Exception ex)
            {
                // File can't be converted

                var audioUploadingResultMessage = new AudioUploadingResultMessage
                {
                    IsSuccess = false,
                    FileName = null,
                    AdditionalData = message.AdditionalData,
                    Exception = ex
                };

                await _audioUploadingMessageBroker
                    .SendFileConversionResult(audioUploadingResultMessage);
                
                return;
            }

            var fileName = AudioFileNameGenerator.GenerateAudioFileName();

            await this._audioStorage.UploadAudioFileAsync(fileName, stream);

            try
            {
                var audioUploadingResultMessage = new AudioUploadingResultMessage
                {
                    IsSuccess = true,
                    FileName = fileName,
                    AdditionalData = message.AdditionalData
                };

                await _audioUploadingMessageBroker.SendFileConversionResult(audioUploadingResultMessage);
            }
            catch
            {
                try
                {
                    await _audioStorage.DeleteAudioFileAsync(fileName);
                }
                catch
                {
                    // TODO: Log that file wasn't cleaned up
                    throw;
                }

                throw;
            }
        }
    }
}
