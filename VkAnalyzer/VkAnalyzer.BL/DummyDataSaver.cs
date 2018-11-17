using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class DummyDataSaver : IDataSaver
    {
        public void SaveData(IEnumerable<UserOnlineInfo> infos)
        {
        }

        public Task SaveDataAsync(IEnumerable<UserOnlineInfo> infos)
        {
            return Task.CompletedTask;
        }
    }
}
