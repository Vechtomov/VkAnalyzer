using System.Collections.Generic;
using System.Threading.Tasks;

namespace VkAnalyzer.BL
{
    public interface IDataSaver
    {
        Task SaveData(IEnumerable<UserOnlineInfo> infos);
    }
}
