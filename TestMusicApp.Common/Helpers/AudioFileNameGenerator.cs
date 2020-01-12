using System;

namespace TestMusicApp.Common.Helpers
{
    public static class AudioFileNameGenerator
    {
        public static string GenerateAudioFileName()
        {
            return $"{Guid.NewGuid()}.mp3";
        }
    }
}
