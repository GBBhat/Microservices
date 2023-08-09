using PlatformServcie.Dtos;

namespace PlatformServcie.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(PlatformPublishDto platformPublishDto);
    }
}