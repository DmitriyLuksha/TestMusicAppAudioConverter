namespace TestMusicApp.AudioConverter.Interfaces.Services
{
    public interface IApplicationConfig
    {
        string ServiceBusConnectionString { get; }

        string AudioConversionQueueName { get; }
    }
}
