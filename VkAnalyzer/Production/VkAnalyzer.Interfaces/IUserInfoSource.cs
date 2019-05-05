using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.Interfaces
{
    public interface IUserInfoSource
    {
        Task<(IEnumerable<UserInfo> users, int count)> SearchUsers(string filter);
        Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> ids);
        Task<IEnumerable<UserInfo>> GetUsersInfo(IEnumerable<long> ids);
	    Task<IEnumerable<UserInfo>> GetUserFriends(long userId);
	}
}
