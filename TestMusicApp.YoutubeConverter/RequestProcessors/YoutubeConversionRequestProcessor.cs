using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger _logger;

        public YoutubeConversionRequestProcessor(
            IYoutubeConversionService youtubeConversionService,
            IAudioStorage audioStorage,
            IAudioUploadingMessageBroker audioUploadingMessageBroker,
            ILogger<YoutubeConversionRequestProcessor> logger
        )
        {
            this._youtubeConversionService = youtubeConversionService;
            this._audioStorage = audioStorage;
            this._audioUploadingMessageBroker = audioUploadingMessageBroker;
            this._logger = logger;
        }

        public async Task<bool> ProcessAsync(YoutubeConversionMessage message)
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
                
                return false;
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
            catch (Exception sendFileResultException)
            {
                _logger.LogError(sendFileResultException, "Send file conversion result");

                try
                {
                    await _audioStorage.DeleteAudioFileAsync(fileName);
                }
                catch (Exception deleteAudioFileException)
                {
                    _logger.LogError(deleteAudioFileException, "Clean up converted audio file");

                    throw;
                }

                throw;
            }

            return true;
        }
    }
}
