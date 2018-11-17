using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.Interfaces
{
    public interface IUserInfoRepository
    {
        void SaveData(IEnumerable<UserOnlineInfo> infos);
        Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos);
        UserOnlineData ReadData(long id, DateTime from, DateTime to);
        Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to);
        UserOnlineData ReadDataByDay(long id, DateTime day);
        Task<UserOnlineData> ReadDataByDayAsync(long id, DateTime day);
    }
}
