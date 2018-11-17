using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.DbModels;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class DbDataSaver 
    {
        public DbDataSaver()
        {
        }

        public void SaveData(IEnumerable<BE.UserOnlineInfo> infos)
        {
            UsersDbContext dbContext = new UsersDbContext();

            var mappedInfos = infos
                .Select(i => new UserOnlineInfo { UserId = i.Id, OnlineInfo = i.OnlineInfo, DateTime = i.DateTime });
            dbContext.UserOnlineInfos.AddRange(mappedInfos);
            dbContext.SaveChanges();
        }

        public async Task SaveDataAsync(IEnumerable<BE.UserOnlineInfo> infos)
        {
            UsersDbContext dbContext = new UsersDbContext();

            var mappedInfos = infos
                .Select(i => new UserOnlineInfo { UserId = i.Id, OnlineInfo = i.OnlineInfo, DateTime = i.DateTime });
            await dbContext.UserOnlineInfos.AddRangeAsync(mappedInfos);
            await dbContext.SaveChangesAsync();
        }
    }
}
