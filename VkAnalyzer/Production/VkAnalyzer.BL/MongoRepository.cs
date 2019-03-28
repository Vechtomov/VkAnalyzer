using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{

	public class MongoRepository : IUserInfoRepository, IUsersRepository
	{
		private readonly IMongoCollection<MongoUser> _collection;

		public MongoRepository(MongoConnectionSettings connectionSettings)
		{
			var client = new MongoClient(new MongoClientSettings
			{
				Server = new MongoServerAddress(connectionSettings.Host, connectionSettings.Port)
			});

			_collection = client.GetDatabase(connectionSettings.DbName).GetCollection<MongoUser>("users");
		}

		public UserOnlineData ReadData(long id, DateTime from, DateTime to)
		{
			return ReadDataV2(id, from, to);
		}

		private UserOnlineData ReadDataV2(long id, DateTime from, DateTime to)
		{
			var data = _collection.Find(u => u.Id == id).Single();

			var infos = data.Info.Where(i => i.DateTime >= from && i.DateTime <= to).ToList();

			var lastBeforeFrom = data.Info.ToList().FindLast(i => i.DateTime < from);

			if(lastBeforeFrom != null)
				infos.Insert(0, lastBeforeFrom);

			return new UserOnlineData
			{
				Id = id,
				OnlineInfos = infos.Select(i => new DateOnline
				{
					OnlineInfo = i.OnlineInfo, Date = i.DateTime
				})
			};
		}

		private UserOnlineData ReadDataV1(long id, DateTime from, DateTime to)
		{
			var userData = _collection
				.Aggregate()
				.Match(u => u.Id == id)
				.Project(Builders<MongoUser>.Projection
					.Expression(u => new UserOnlineData
					{
						Id = u.Id,
						OnlineInfos = u.Info
								.Where(i => i.DateTime >= from && i.DateTime <= to)
								.Select(i => new DateOnline
								{
									OnlineInfo = i.OnlineInfo,
									Date = i.DateTime
								})
					}
					)
				)
				.Single();

			return userData;
		}

		public Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to)
		{
			return Task.FromResult(ReadData(id, from, to));
		}

		public void SaveData(IEnumerable<UserOnlineInfo> infos)
		{
			foreach (var userOnlineInfo in infos)
			{
				var update = Builders<MongoUser>.Update.Push(u => u.Info, new MongoOnlineInfo
				{
					DateTime = userOnlineInfo.DateTime,
					OnlineInfo = userOnlineInfo.OnlineInfo
				});

				_collection.UpdateOne(u => u.Id == userOnlineInfo.Id, update);
			}
		}

		public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
		{
			foreach (var userOnlineInfo in infos)
			{
				var update = Builders<MongoUser>.Update.Push(u => u.Info, Map(userOnlineInfo));

				await _collection.UpdateOneAsync(u => u.Id == userOnlineInfo.Id, update);
			}
		}

		public IEnumerable<User> GetUsers()
		{
			return _collection.Find(u => true).ToList().Select(Map);
		}

		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			return (await _collection.Find(u => true).ToListAsync()).Select(Map);
		}

		public void AddUser(User user)
		{
			_collection.InsertOne(Map(user));
		}

		public async Task AddUserAsync(User user)
		{
			await _collection.InsertOneAsync(Map(user));
		}

		private static User Map(MongoUser user)
		{
			return new User
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AddedDateTime = user.AddedDateTime,
				AddedUser = user.AddedUser
			};
		}

		private static MongoUser Map(User user)
		{
			return new MongoUser
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AddedDateTime = user.AddedDateTime,
				AddedUser = user.AddedUser,
				Info = new List<MongoOnlineInfo>
				{
					new MongoOnlineInfo {DateTime = DateTime.Now, OnlineInfo = OnlineInfo.Undefined}
				}
			};
		}

		private static MongoOnlineInfo Map(UserOnlineInfo userOnlineInfo)
		{
			return new MongoOnlineInfo
			{
				DateTime = userOnlineInfo.DateTime,
				OnlineInfo = userOnlineInfo.OnlineInfo
			};
		}
	}
}
