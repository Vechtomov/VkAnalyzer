using System;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.Interfaces
{
    public interface IDataReader
    {
        UserOnlineData ReadData(long id, DateTime from, DateTime to);
        Task<UserOnlineData> ReadDataAsync(long id, DateTime from, DateTime to);
        UserOnlineData ReadDataByDay(long id, DateTime day);
        Task<UserOnlineData> ReadDataByDayAsync(long id, DateTime day);
    }
}
