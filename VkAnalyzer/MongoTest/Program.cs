using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MongoTest
{
	public class Program
	{
		private static IMongoDatabase _db;

		public static void Main(string[] args)
		{
			var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
			ConventionRegistry.Register("camelCase", conventionPack, t => true);

			const string host = "localhost";
			const int port = 27017;
			var client = new MongoClient(new MongoClientSettings
			{
				Server = new MongoServerAddress(host, port)
			});

			const string dbName = "VkTracker";
			_db = client.GetDatabase(dbName);

			var task = Task.Factory.StartNew(async () =>
			{
				var stopwatch = new Stopwatch();
				stopwatch.Start();

				//await InsertRandomInfo(1);
				//await InsertRandomInfo(2);
				//await InsertRandomInfo(3);
				//await InsertRandomInfo(4);
				//await InsertRandomInfo(5);

				var userInfo = GetUserInfo(1, DateTime.Now.AddHours(-100));
				stopwatch.Stop();
				var elapsedTime = stopwatch.ElapsedMilliseconds;

				Console.WriteLine($"Elapsed time: {elapsedTime} ms.");

				return userInfo;
			});


			var mongoUsers = task.Result.Result;

			Console.WriteLine(mongoUsers);
			Console.ReadLine();
		}

		private static async Task<IEnumerable<User>> GetUsersAsync()
		{
			var collection = _db.GetCollection<User>("users");

			var users = await collection.FindAsync(x => true);

			return await users.ToListAsync();
		}

		private static async Task InsertRandomInfo(long id)
		{
			var collection = _db.GetCollection<User>("users");

			var rand = new Random();
			var data = GetRandomData(1000).Select(d => new {Id = rand.Next(1,6), Info = d });

			var update = Builders<User>.Update.PushEach(u => u.Info, data.Select(d => d.Info));

			await collection.UpdateOneAsync(x => x.Id == id, update);
		}

		private static IEnumerable<Info> GetRandomData(int count)
		{
			var rand = new Random();

			return Enumerable.Range(0, count)
				.Select(i => new Info
				{
					DateTime = DateTime.Now.AddHours(-count + i),
					OnlineInfo = (OnlineInfo) rand.Next(3)
				});
		}

		private static User GetUserInfo(long id, DateTime dateTime)
		{
			var collection = _db.GetCollection<User>("users");

			return collection.Aggregate().Match(u => u.Id == id).Project(Builders<User>.Projection.Expression(u => new User
			{
				Id = u.Id,
				Info = u.Info.Where(i => i.DateTime > dateTime)
			})).Single();
		}
	}

	[BsonIgnoreExtraElements]
	public class User
	{
		[BsonId]
		public long Id { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }

		public IEnumerable<Info> Info { get; set; }
	}

	public class Info
	{
		public DateTime DateTime { get; set; }
		public OnlineInfo OnlineInfo { get; set; }
	}

	public enum OnlineInfo
	{
		Offline,
		Online,
		OnlineMobile
	}
}
