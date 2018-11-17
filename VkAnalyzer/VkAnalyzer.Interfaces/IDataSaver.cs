using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.Interfaces
{
    public interface IDataSaver
    {
        void SaveData(IEnumerable<UserOnlineInfo> infos);
        Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos);
    }
}
