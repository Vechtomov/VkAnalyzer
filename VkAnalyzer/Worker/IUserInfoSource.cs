using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.BL
{
    public interface IUserInfoSource
    {
        Task<bool> TryInit(string login, string password);
        Task<(IEnumerable<UserInfo> users, int count)> SearchUsers(string filter);
        Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> id);
    } 
}
