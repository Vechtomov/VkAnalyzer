using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VkAnalyzer.BE;
using VkAnalyzer.BL.SQL;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL.Sql
{
	public class SqlRepository : IUserInfoRepository, IUsersRepository
	{
		private readonly DbContextOptions<UsersDbContext> _options;

		public SqlRepository(DbContextOptions<UsersDbContext> options)
		{
			_options = options;
		}

		public void SaveData(IEnumerable<UserOnlineInfo> infos)
		{
			var dbContext = new UsersDbContext(_options);
			dbContext.UserOnlineInfos.AddRange(infos.Select(info => new UserOnlineInfoModel
			{
				OnlineInfo = info.OnlineInfo,
				DateTime = info.DateTime,
				UserId = info.Id
			}));
			dbContext.SaveChanges();
		}

		public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
		{
			var dbContext = new UsersDbContext(_options);
			await dbContext.UserOnlineInfos.AddRangeAsync(infos.Select(info => new UserOnlineInfoModel
			{
				OnlineInfo = info.OnlineInfo,
				DateTime = info.DateTime,
				UserId = info.Id
			}));
			await dbContext.SaveChangesAsync();
		}

		public UserOnlineData ReadData(long id, DateTime from, DateTime to)
		{
			var dbContext = new UsersDbContext(_options);
			return new UserOnlineData
			{
				Id = id,
				OnlineInfos = dbContext.UserOnlineInfos
					.Where(info => info.UserId == id && info.DateTime >= from && info.DateTime <= to)
					.Select(info => new DateOnline
					{
						OnlineInfo = info.OnlineInfo,
						Date = info.DateTime
					})
			};
		}

		public Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to)
		{
			var dbContext = new UsersDbContext(_options);
			return Task.FromResult(new UserOnlineData
			{
				Id = id,
				OnlineInfos = dbContext.UserOnlineInfos
					.Where(info => info.UserId == id && info.DateTime >= from && info.DateTime <= to)
					.Select(info => new DateOnline
					{
						OnlineInfo = info.OnlineInfo,
						Date = info.DateTime
					})
			});
		}

		public int GetUsersCount()
		{
			using (var dbContext = new UsersDbContext(_options))
			{
				return dbContext.Users.Count();
			}
		}

		public async Task<int> GetUsersCountAsync()
		{
			using (var dbContext = new UsersDbContext(_options))
			{
				return await dbContext.Users.CountAsync();
			}
		}

		public IEnumerable<User> GetUsers()
		{
			var dbContext = new UsersDbContext(_options);
			return dbContext.Users.ToList();
		}

		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			var dbContext = new UsersDbContext(_options);
			return await dbContext.Users.ToListAsync();
		}

		public void AddUser(User user)
		{
			var dbContext = new UsersDbContext(_options);
			dbContext.Users.Add(user);
			dbContext.SaveChanges();
		}

		public async Task AddUserAsync(User user)
		{
			var dbContext = new UsersDbContext(_options);
			await dbContext.Users.AddAsync(user);
			await dbContext.SaveChangesAsync();
		}
	}
}
