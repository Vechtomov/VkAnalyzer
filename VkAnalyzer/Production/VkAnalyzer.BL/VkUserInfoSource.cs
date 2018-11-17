using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.BE;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using VkAnalyzer.Interfaces;

namespace VkAnalyzer.BL
{
    public class VkUserInfoSource : IUserInfoSource
    {
        private ulong AppId;

        /// <summary>
        /// API для работы с вк
        /// </summary>
        private VkApi vkApi = new VkApi();

        public VkUserInfoSource(VkAnalyzerSettings settings)
        {
            AppId = settings.AppId;
            ApiAuthParams param = new ApiAuthParams
            {
                ApplicationId = AppId,
                Login = settings.VkUserLogin,
                Password = settings.VkUserPassword,
                Settings = Settings.All
            };

            vkApi.Authorize(param);
        }

        public async Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> ids)
        {
            if (ids == null)
            {
                return null;
            }

            // Находим пользователя
            var users = await vkApi.Users.GetAsync(ids.ToArray(), ProfileFields.Online | ProfileFields.OnlineMobile | ProfileFields.OnlineApp);
            var dateTime = DateTime.Now;

            return users.Select(u => new UserOnlineInfo
            {
                Id = u.Id,
                DateTime = dateTime,
                OnlineInfo = GetOnlineInfoByUser(u)
            });
        }

        private OnlineInfo GetOnlineInfoByUser(User user)
        {
            bool online = user.Online.HasValue && user.Online.Value;
            bool onlineMobile = user.OnlineMobile.HasValue && user.OnlineMobile.Value;
            bool onlineApp = user.OnlineApp.HasValue;

            return onlineMobile
                ? OnlineInfo.OnlineMobile
                : onlineApp
                    ? OnlineInfo.OnlineApp
                    : online
                        ? OnlineInfo.Online
                        : OnlineInfo.Offline;
        }

        public async Task<(IEnumerable<UserInfo> users, int count)> SearchUsers(string filter)
        {
            if (filter.StartsWith("id"))
            {
                var temp = filter.Substring(2);
                if (long.TryParse(temp, out var id))
                {
                    return (await GetUsersInfo(new long[] { id }), 1);
                }

            }
            var users = await vkApi.Users.SearchAsync(new UserSearchParams
            {
                Query = filter,
                Fields = ProfileFields.Photo100 | ProfileFields.ScreenName
            });

            return (users.Select(u => new UserInfo
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ScreenName = u.ScreenName,
                Photo = u.Photo100.ToString(),
            }), (int)users.TotalCount);
        }

        public async Task<IEnumerable<UserInfo>> GetUsersInfo(IEnumerable<long> ids)
        {
            var user = await vkApi.Users.GetAsync(ids, ProfileFields.Photo100 | ProfileFields.ScreenName);
            return user.Select(u => new UserInfo
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                ScreenName = u.ScreenName,
                Photo = u.Photo100.ToString(),
            });
        }
    }
}
