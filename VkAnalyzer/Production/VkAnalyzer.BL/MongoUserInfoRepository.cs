using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class MongoUserInfoRepository : IUserInfoRepository
    {
        private MongoClient _client;

        public MongoUserInfoRepository(MongoConnectionSettings connectionSettings)
        {
            var url = $"mongodb://{connectionSettings.Host}:{connectionSettings.Port}";
            _client = new MongoClient(url);
        }

        public UserOnlineData ReadData(long id, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to)
        {
            throw new NotImplementedException();
        }

        public UserOnlineData ReadDataByDay(long id, DateTime day)
        {
            throw new NotImplementedException();
        }

        public Task<UserOnlineData> ReadDataByDayAsync(long id, DateTime day)
        {
            throw new NotImplementedException();
        }

        public void SaveData(IEnumerable<UserOnlineInfo> infos)
        {
            throw new NotImplementedException();
        }

        public Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
        {
            throw new NotImplementedException();
        }
    }
}
