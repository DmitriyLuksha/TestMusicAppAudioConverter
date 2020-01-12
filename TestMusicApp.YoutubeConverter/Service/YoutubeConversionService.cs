using System.IO;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using TestMusicApp.Common.Configs;
using VideoLibrary;

namespace TestMusicApp.YoutubeConverter.Service
{
    public class YoutubeConversionService : IYoutubeConversionService
    {
        private readonly IConversionConfig _conversionConfig;

        public YoutubeConversionService(IConversionConfig conversionConfig)
        {
            this._conversionConfig = conversionConfig;
        }

        private const string VideoUrlFormat = "https://www.youtube.com/watch?v={0}";

        public async Task ConvertVideoToMp3Async(string videoId, Stream target)
        {
            var folder = _conversionConfig.TempFilesFolder;

            Directory.CreateDirectory(folder);

            var tempVideoFilePath = $"{folder}/temp_{videoId}";
            var convertedFilePath = $"{folder}/{videoId}.mp3";

            try
            {
                await ReadVideoIntoFileAsync(videoId, tempVideoFilePath);

                ConvertVideoToMp3(tempVideoFilePath, convertedFilePath);

                target.Position = 0;

                using (var audioStream = File.OpenRead(convertedFilePath))
                {
                    await audioStream.CopyToAsync(target);
                }
            }
            catch
            {
                DeleteIfExists(tempVideoFilePath);
                DeleteIfExists(convertedFilePath);

                throw;
            }

            DeleteIfExists(tempVideoFilePath);
            DeleteIfExists(convertedFilePath);
        }

        private string GetVideoUrl(string videoId)
        {
            return string.Format(VideoUrlFormat, videoId);
        }

        private async Task ReadVideoIntoFileAsync(string videoId, string path)
        {
            var youtube = YouTube.Default;
            var videoUrl = GetVideoUrl(videoId);
            var video = await youtube.GetVideoAsync(videoUrl);
            var bytes = await video.GetBytesAsync();

            await File.WriteAllBytesAsync(path, bytes);
        }

        private void ConvertVideoToMp3(string videoPath, string outputPath)
        {
            var inputFile = new MediaFile { Filename = videoPath };
            var outputFile = new MediaFile { Filename = outputPath };

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);
                engine.Convert(inputFile, outputFile);
            }
        }

        private void DeleteIfExists(string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
    }
}
