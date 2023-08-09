using CommandsService.Models;

namespace CommandsService.CommandData
{
    public interface IcommandRepo
    {
        bool SaveChanges();
        IEnumerable<Platform> GetAllPlatforms();
        void CreatePlatform(Platform plat);
        bool PlatformExists(int platformId);

        bool ExternalPlatformExists(int externalPlatformId);

        //commands
        IEnumerable<Command> GetCommandsForPlatform(int platformId);
        Command GetCommand(int platformId, int commandId);
        void CreateCommand(int platformId, Command command);


    }
}