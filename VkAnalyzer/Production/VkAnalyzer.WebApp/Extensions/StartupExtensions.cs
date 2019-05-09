using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using NLog;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.BL.File;
using VkAnalyzer.BL.Sql;
using VkAnalyzer.Interfaces;
using VkAnalyzer.WebApp.Settings;
using VkNet.NLog.Extensions.Logging;
using VkNet.NLog.Extensions.Logging.Extensions;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace VkAnalyzer.WebApp.Extensions
{
	public static class StartupExtensions
	{
		public static void AddVkUserInfoSource(this IServiceCollection collection)
		{
			collection.AddSingleton<IUserInfoSource, VkUserInfoSource>(x =>
			{
				var vkAnalyzerSettings = x.GetService<IOptions<VkAnalyzerSettings>>().Value;

				return new VkUserInfoSource(vkAnalyzerSettings);
			});
		}

		public static void AddDummyUserInfoSource(this IServiceCollection collection)
		{
			collection.AddSingleton<IUserInfoSource, DummyUsersInfoSource>();
		}

		public static void AddUserInfoSource(this IServiceCollection collection)
		{
			var mode = collection.BuildServiceProvider().GetService<IOptions<UserInfoSourceMode>>().Value;

			switch (mode.Mode)
			{
				case "Vk":
					collection.AddVkUserInfoSource();
					break;
				case "Dummy":
					collection.AddDummyUserInfoSource();
					break;
				default:
					throw new ArgumentException(nameof(UserInfoSourceMode));
			}
		}


		public static void AddFileRepository(this IServiceCollection collection)
		{
			collection.AddSingleton<IUsersRepository, FileUsersRepository>(x =>
			{
				var fileRepositorySettings = x.GetService<IOptions<FileRepositorySettings>>().Value;
				return new FileUsersRepository(fileRepositorySettings);
			});
			collection.AddSingleton<IUserInfoRepository, FileRepository>(x =>
			{
				var fileRepositorySettings = x.GetService<IOptions<FileRepositorySettings>>().Value;
				return new FileRepository(fileRepositorySettings);
			});
		}

		public static void AddSqlRepository(this IServiceCollection collection)
		{
			var serviceProvider = collection.BuildServiceProvider();

			var repositorySettings = serviceProvider.GetService<IOptions<SqlRepositorySettings>>().Value;

			collection.AddDbContext<UsersDbContext>(o => o.UseSqlServer(repositorySettings.Connection));

			var options = serviceProvider.GetRequiredService<DbContextOptions<UsersDbContext>>();

			var repository = new SqlRepository(options);

			collection.AddSingleton<IUserInfoRepository, SqlRepository>(x => repository);
			collection.AddSingleton<IUsersRepository, SqlRepository>(x => repository);
		}

		public static void AddMongoRepository(this IServiceCollection collection)
		{
			// register convention
			var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
			ConventionRegistry.Register("camelCase", conventionPack, t => true);

			// get settings
			var serviceProvider = collection.BuildServiceProvider();
			var repositorySettings = serviceProvider.GetService<IOptions<MongoConnectionSettings>>().Value;

			var repository = new MongoRepository(repositorySettings);
			collection.AddSingleton<IUsersRepository, MongoRepository>(x => repository);
			collection.AddSingleton<IUserInfoRepository, MongoRepository>(x => repository);
		}

		public static void AddRepository(this IServiceCollection collection)
		{
			var mode = collection.BuildServiceProvider().GetService<IOptions<RepositoryMode>>().Value;

			switch (mode.Mode)
			{
				case "Sql":
					collection.AddSqlRepository();
					break;
				case "File":
					collection.AddFileRepository();
					break;
				case "Mongo":
					collection.AddMongoRepository();
					break;
				default:
					throw new ArgumentException(nameof(RepositoryMode));
			}
		}

		public static void AddLogger(this IServiceCollection services)
		{
			services.AddSingleton<ILoggerFactory, LoggerFactory>();
			services.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
			services.AddLogging(builder =>
			{
				builder.ClearProviders();
				builder.SetMinimumLevel(LogLevel.Trace);
				builder.AddNLog(new NLogProviderOptions
				{
					CaptureMessageProperties = true,
					CaptureMessageTemplates = true
				});
			});
			LogManager.LoadConfiguration("nlog.config");
		}
	}
}
