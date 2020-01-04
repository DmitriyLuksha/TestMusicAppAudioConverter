namespace TestMusicApp.AudioConverter.Messages
{
    public class AudioConversionResultMessage
    {
        public bool IsSuccess { get; set; }

        public string FileName { get; set; }

        public object AdditionalData { get; set; }
    }
}
