using Microsoft.EntityFrameworkCore;

namespace VkAnalyzer.DbModels
{
    public class UsersDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserOnlineInfo> UserOnlineInfos { get; set; }

        //public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
        //{
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.UseSqlServer($"Data Source=DESKTOP-C7FAO2K\\SQLEXPRESS;Initial Catalog={nameof(UsersDbContext)};Integrated Security=True");

            base.OnConfiguring(builder);
        }

    }
}
