using System.Threading.Tasks;
using PlatformServcie.Dtos;

namespace PlatformServcie.SyncDataClient.Http
{
    public interface ICommandDataClient
    {
        Task SendPlatformtoCommand(PlatformReadDto plat);
    }
}