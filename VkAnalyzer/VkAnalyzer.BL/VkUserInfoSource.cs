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

namespace VkAnalyzer.BL
{
    public class VkUserInfoSource : IUserInfoSource
    {
        private ulong AppId;

        /// <summary>
        /// API для работы с вк
        /// </summary>
        private VkApi vkApi = new VkApi();

        public VkUserInfoSource()
        {
            var lines = File.ReadAllLines("settings.txt");
            AppId = ulong.Parse(lines[0]);
        }

        public VkUserInfoSource(string login, string password) : this()
        {
            ApiAuthParams param = new ApiAuthParams
            {
                ApplicationId = AppId,
                Login = login,
                Password = password,
                Settings = Settings.All
            };

            vkApi.Authorize(param);
        }

        public async Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> ids)
        {
            // Находим пользователя
            var users = await vkApi.Users.GetAsync(ids.ToArray(), ProfileFields.Online | ProfileFields.OnlineMobile | ProfileFields.OnlineApp);
            return users.Select(u => new UserOnlineInfo
            {
                Id = u.Id,
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

        public async Task<bool> TryInit(string login, string password)
        {
            ApiAuthParams param = new ApiAuthParams
            {
                ApplicationId = AppId,
                Login = login,
                Password = password,
                Settings = Settings.All
            };

            try
            {
                await vkApi.AuthorizeAsync(param);
            }
            catch (Exception)
            {
                // log
                return false;
            }

            return true;
        }

        public async Task<(IEnumerable<UserInfo> users, int count)> SearchUsers(string filter)
        {
            if (filter.StartsWith("id"))
            {
                var temp = filter.Substring(2);
                if (long.TryParse(temp, out var id))
                {
                    var user = await vkApi.Users.GetAsync(new long[] { id }, ProfileFields.Photo100 | ProfileFields.ScreenName);
                    return (user.Select(u => new UserInfo
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        ScreenName = u.ScreenName,
                        Photo = u.Photo100.ToString(),
                    }), user.Count);
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
    }
}
