using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using VkAnalyzer.BE;
using VkAnalyzer.BL;
using VkAnalyzer.BL.File;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.Extensions
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
    }
}
