using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;
using VkNet.Model.RequestParams;
using User = VkNet.Model.User;

namespace VkAnalyzer.BL
{
	public class VkUserInfoSource : IUserInfoSource
	{
		/// <summary>
		/// API для работы с вк
		/// </summary>
		private readonly VkApi _vkApi;

		public VkUserInfoSource(VkAnalyzerSettings settings, IServiceCollection services = null)
		{
			_vkApi = new VkApi(services);
			var param = new ApiAuthParams
			{
				ApplicationId = ulong.Parse(settings.AppId),
				Login = settings.VkUserLogin,
				Password = settings.VkUserPassword,
				Settings = Settings.Status,
			};

			_vkApi.OnTokenExpires += sender =>
			{
				sender.RefreshToken();
				Debug.WriteLine("Refresh token");
			};

			_vkApi.Authorize(param);
		}

		public async Task<IEnumerable<UserOnlineInfo>> GetOnlineInfo(IEnumerable<long> ids)
		{
			if (ids == null)
			{
				return null;
			}

			// Находим пользователя
			var users = await _vkApi.Users.GetAsync(ids.ToArray(), ProfileFields.Online | ProfileFields.OnlineMobile | ProfileFields.OnlineApp | ProfileFields.LastSeen);
			var dateTime = DateTime.Now.ToUniversalTime();

			return users.Where(u => !u.IsDeactivated).Select(u => new UserOnlineInfo
			{
				Id = u.Id,
				DateTime = GetOnlineInfoByUser(u) == OnlineInfo.Offline && u.LastSeen.Time.HasValue
					? u.LastSeen.Time.Value.ToUniversalTime()
					: dateTime,
				OnlineInfo = GetOnlineInfoByUser(u)
			});
		}

		private static OnlineInfo GetOnlineInfoByUser(User user)
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
			if (string.IsNullOrEmpty(filter))
				return (null, 0);

			if (filter.StartsWith("id"))
			{
				var temp = filter.Substring(2);
				if (long.TryParse(temp, out var id))
				{
					return (await GetUsersInfo(new[] { id }), 1);
				}
			}

			var users = await _vkApi.Users.SearchAsync(new UserSearchParams
			{
				Query = filter,
				Fields = ProfileFields.Photo100 | ProfileFields.ScreenName
			});

			return (users.Select(ConvertUserToUserInfo), (int)users.TotalCount);
		}

		public async Task<IEnumerable<UserInfo>> GetUsersInfo(IEnumerable<long> ids)
		{
			var result = new List<User>();
			var userIds = ids.ToList();

			const int maxUsersCountPerRequest = 1000;

			for (var i = 0; i < userIds.Count / maxUsersCountPerRequest + 1; i++)
			{
				var users = await _vkApi.Users.GetAsync(userIds.Skip(i * maxUsersCountPerRequest).Take(maxUsersCountPerRequest),
					ProfileFields.Photo100 | ProfileFields.ScreenName | ProfileFields.Counters,
					skipAuthorization: true);
				result.AddRange(users);
			}
			return result.Select(ConvertUserToUserInfo);
		}

		public async Task<IEnumerable<UserInfo>> GetUserFriends(long userId)
		{
			var friends = await _vkApi.Friends.GetAsync(new FriendsGetParams
			{
				UserId = userId,
				Fields = ProfileFields.Photo100 | ProfileFields.ScreenName,
			});

			return friends.Select(ConvertUserToUserInfo);
		}

		private static UserInfo ConvertUserToUserInfo(User user)
		{
			return new UserInfo
			{
				Id = user.Id,
				FirstName = user.FirstName,
				LastName = user.LastName,
				ScreenName = user.ScreenName,
				Photo = user.Photo100.ToString(),
				FriendsCount = user.Counters?.Friends
			};
		}
	}
}
