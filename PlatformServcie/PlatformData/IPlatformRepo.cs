using System.Collections.Generic;
using PlatformServcie.Models;

namespace PlatformServcie.PlatformData
{
    public interface IPlatformRepo
    {
        bool SaveChanges();

        IEnumerable<Platform> GetAllPlatforms();
        Platform GetPlatformById(int id);
        void CreatePlatform(Platform plat);
    }
}