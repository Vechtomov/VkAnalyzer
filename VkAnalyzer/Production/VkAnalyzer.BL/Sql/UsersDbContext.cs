using Microsoft.EntityFrameworkCore;
using VkAnalyzer.BE;
using VkAnalyzer.BL.Sql;

namespace VkAnalyzer.BL.SQL
{
	public sealed class UsersDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<UserOnlineInfoModel> UserOnlineInfos { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<User>()
				.HasIndex(b => b.Id)
				.IsUnique();

			modelBuilder.Entity<UserOnlineInfoModel>()
				.HasIndex(b => b.UserId);

			modelBuilder.Entity<UserOnlineInfoModel>()
				.HasIndex(b => b.DateTime);
				
		}

		public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options)
		{
			Database.EnsureCreated();
		}
	}
}
