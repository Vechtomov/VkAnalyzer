using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL
{
    public interface IDataSaver
    {
        Task SaveData(IEnumerable<UserOnlineInfo> infos);
    }
}
