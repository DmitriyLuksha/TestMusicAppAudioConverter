using System;

namespace TestMusicApp.Common.Messages
{
    public class AudioUploadingResultMessage
    {
        public bool IsSuccess { get; set; }

        public string FileName { get; set; }

        public object AdditionalData { get; set; }

        public Exception Exception { get; set; }
    }
}
