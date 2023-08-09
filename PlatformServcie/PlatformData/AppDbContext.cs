using Microsoft.EntityFrameworkCore;
using PlatformServcie.Models;

namespace PlatformServcie.PlatformData
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opt) :base (opt)
        {
            
        }

        public DbSet<Platform> Platforms { get; set; }
    }
}