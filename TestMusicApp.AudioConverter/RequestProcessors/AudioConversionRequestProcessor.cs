using System;
using System.IO;
using System.Threading.Tasks;
using TestMusicApp.AudioConverter.Messages;
using TestMusicApp.AudioConverter.Services;
using TestMusicApp.AudioConverter.Storages;
using TestMusicApp.Common.Helpers;
using TestMusicApp.Common.MessageBrokers;
using TestMusicApp.Common.Messages;
using TestMusicApp.Common.Storages;

namespace TestMusicApp.AudioConverter.RequestProcessors
{
    public class AudioConversionRequestProcessor : IAudioConversionRequestProcessor
    {
        private readonly IAudioConversionService _audioConversionService;
        private readonly IUnprocessedAudioFilesStorage _unprocessedAudioFilesStorage;
        private readonly IAudioStorage _audioStorage;
        private readonly IAudioUploadingMessageBroker _audioUploadingMessageBroker;

        public AudioConversionRequestProcessor(
            IAudioConversionService audioConversionService,
            IUnprocessedAudioFilesStorage unprocessedAudioFilesStorage,
            IAudioStorage audioStorage,
            IAudioUploadingMessageBroker audioUploadingMessageBroker
        )
        {
            this._audioConversionService = audioConversionService;
            this._unprocessedAudioFilesStorage = unprocessedAudioFilesStorage;
            this._audioStorage = audioStorage;
            this._audioUploadingMessageBroker = audioUploadingMessageBroker;
        }
        
        public async Task ProcessAsync(AudioConversionMessage message)
        {
            var fileName = message.FileName;

            var unprocessedAudioFileContent = new MemoryStream();
            var result = new MemoryStream();

            await _unprocessedAudioFilesStorage.ReadUnprocessedAudioFileAsync(fileName, unprocessedAudioFileContent);
            
            try
            {
                _audioConversionService.ConvertToMp3(unprocessedAudioFileContent, result);
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

                await CleanUpUnprocessedFileAsync(fileName);

                return;
            }

            var newFileName = AudioFileNameGenerator.GenerateAudioFileName();

            await _audioStorage.UploadAudioFileAsync(newFileName, result);

            try
            {
                var audioUploadingResultMessage = new AudioUploadingResultMessage
                {
                    IsSuccess = true,
                    FileName = newFileName,
                    AdditionalData = message.AdditionalData
                };

                await _audioUploadingMessageBroker.SendFileConversionResult(audioUploadingResultMessage);
            }
            catch
            {
                try
                {
                    await _audioStorage.DeleteAudioFileAsync(newFileName);
                }
                catch
                {
                    // TODO: Log that file wasn't cleaned up
                    throw;
                }

                throw;
            }

            await CleanUpUnprocessedFileAsync(fileName);
        }

        private async Task CleanUpUnprocessedFileAsync(string fileName)
        {
            try
            {
                await _unprocessedAudioFilesStorage.DeleteUnprocessedAudioFileAsync(fileName);
            }
            catch
            {
                // TODO Log that file wasn't cleaned up

                // Don't bubble exception because file was processed
                // we just can't clean up old file
                // so we don't want message be marked as dead letter
            }
        }
    }
}
