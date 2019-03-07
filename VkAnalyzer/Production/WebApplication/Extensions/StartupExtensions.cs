using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.BL.File;
using VkAnalyzer.BL.Sql;
using VkAnalyzer.BL.SQL;
using VkAnalyzer.Interfaces;
using WebApplication.Settings;

namespace WebApplication.Extensions
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
	        var repositorySettings = collection.BuildServiceProvider().GetService<IOptions<SqlRepositorySettings>>().Value;
			collection.AddDbContext<UsersDbContext>(o => o.UseSqlServer(repositorySettings.Connection));
	        var options = collection.BuildServiceProvider().GetRequiredService<DbContextOptions<UsersDbContext>>();
			var repository = new SqlRepository(options);
			collection.AddSingleton<IUserInfoRepository, SqlRepository>(x => repository);
	        collection.AddSingleton<IUsersRepository, SqlRepository>(x => repository);
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
			    default:
				    throw new ArgumentException(nameof(RepositoryMode));
		    }
		}
	}
}
