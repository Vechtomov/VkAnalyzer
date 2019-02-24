﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class DummyUsersInfoSource : IUserInfoSource
    {
        public Task<(IEnumerable<UserInfo> users, int count)> SearchUsers(string filter)
        {
            return Task.FromResult((Enumerable.Empty<UserInfo>(), 0));
        }

        public Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> ids)
        {
            return Task.FromResult(ids.Select(id => new UserOnlineInfo()
            {
                DateTime = DateTime.Now,
                Id = id,
                OnlineInfo = GetRandomOnlineInfoById(id)
            }));
        }

        public Task<IEnumerable<UserInfo>> GetUsersInfo(IEnumerable<long> ids)
        {
            return Task.FromResult(ids.Select(id => new UserInfo()
            {
                Id = id,
            }));
        }

        private static OnlineInfo GetRandomOnlineInfoById(long id)
        {
            return (OnlineInfo)((DateTime.Now.Hour + id) % 5);
        }
    }
}