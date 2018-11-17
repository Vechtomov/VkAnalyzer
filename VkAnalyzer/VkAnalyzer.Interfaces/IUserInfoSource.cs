﻿using System.Collections.Generic;
using System.Threading.Tasks;
using VkAnalyzer.BE;

namespace VkAnalyzer.Interfaces
{
    public interface IUserInfoSource
    {
        Task<bool> TryInit(string login, string password);
        Task<(IEnumerable<UserInfo> users, int count)> SearchUsers(string filter);
        Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> id);
        Task<IEnumerable<UserInfo>> GetUsersInfo(IEnumerable<long> ids);
    }
}