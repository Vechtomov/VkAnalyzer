using Microsoft.EntityFrameworkCore;
using VkAnalyzer.DbModels;

namespace VkAnalyzer.BL
{
    public sealed class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserOnlineInfo> UserOnlineInfos { get; set; }

        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
