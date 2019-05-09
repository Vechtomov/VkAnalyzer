using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{

	public class MongoRepository : IUserInfoRepository, IUsersRepository
	{
		private const int DefaultMongoPort = 27017;
		private readonly IMongoDatabase _db;
		private IMongoCollection<MongoUser> Users => _db.GetCollection<MongoUser>("users");

		public MongoRepository(MongoConnectionSettings connectionSettings)
		{
			var parsed = int.TryParse(connectionSettings.Port, out var port);
			var client = new MongoClient(new MongoClientSettings
			{
				Server = new MongoServerAddress(connectionSettings.Host, parsed ? port : DefaultMongoPort)
			});

			_db = client.GetDatabase(connectionSettings.Database);
			if (!CollectionExists(_db, "users"))
			{
				_db.CreateCollection("users");
			}
		}

		public bool CollectionExists(IMongoDatabase database, string collectionName)
		{
			var filter = new BsonDocument("name", collectionName);
			var options = new ListCollectionNamesOptions { Filter = filter };

			return database.ListCollectionNames(options).Any();
		}


		public UserOnlineData ReadData(long id, DateTime from, DateTime to)
		{
			return ReadDataV2(id, from, to);
		}

		private UserOnlineData ReadDataV2(long id, DateTime from, DateTime to)
		{
			var data = Users.Find(u => u.Id == id).Single();

			var infos = data.Info.Where(i => i.DateTime >= from && i.DateTime <= to).ToList();

			var lastBeforeFrom = data.Info.ToList().FindLast(i => i.DateTime < from);

			if (lastBeforeFrom != null)
				infos.Insert(0, lastBeforeFrom);

			return new UserOnlineData
			{
				Id = id,
				OnlineInfos = infos.Select(i => new DateOnline
				{
					OnlineInfo = i.OnlineInfo,
					Date = i.DateTime
				})
			};
		}

		private UserOnlineData ReadDataV1(long id, DateTime from, DateTime to)
		{
			var userData = Users
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

				Users.UpdateOne(u => u.Id == userOnlineInfo.Id, update);
			}
		}

		public async Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
		{
			foreach (var userOnlineInfo in infos)
			{
				var update = Builders<MongoUser>.Update.Push(u => u.Info, Map(userOnlineInfo));

				await Users.UpdateOneAsync(u => u.Id == userOnlineInfo.Id, update);
			}
		}

		public int GetUsersCount()
		{
			return (int)Users.CountDocuments(Builders<MongoUser>.Filter.Empty);
		}

		public async Task<int> GetUsersCountAsync()
		{
			return (int)await Users.CountDocumentsAsync(Builders<MongoUser>.Filter.Empty);
		}

		public IEnumerable<User> GetUsers()
		{
			return Users.Find(u => true).ToList().Select(Map);
		}

		public async Task<IEnumerable<User>> GetUsersAsync()
		{
			return (await Users.Find(u => true).ToListAsync()).Select(Map);
		}

		public void AddUser(User user)
		{
			Users.InsertOne(Map(user));
		}

		public async Task AddUserAsync(User user)
		{
			await Users.InsertOneAsync(Map(user));
		}

		private static User Map(MongoUser user)
		{
			return new User
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				AddedDateTime = user.AddedDateTime,
				ScreenName = user.ScreenName,
				AdditionalInfo = user.AdditionalInfo,
				Photo = user.Photo,
				LastOnline = user.LastOnline
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
				AdditionalInfo = user.AdditionalInfo,
				Photo = user.Photo,
				ScreenName = user.ScreenName,
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
