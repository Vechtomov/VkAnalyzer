using System;
using System.Collections.Generic;
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
				Settings = Settings.Status
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

			return users.Select(u => new UserOnlineInfo
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

			return (users.Select(u => new UserInfo
			{
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				ScreenName = u.ScreenName,
				Photo = u.Photo100.ToString()
			}), (int)users.TotalCount);
		}

		public async Task<IEnumerable<UserInfo>> GetUsersInfo(IEnumerable<long> ids)
		{
			var user = await _vkApi.Users.GetAsync(ids, ProfileFields.Photo100 | ProfileFields.ScreenName);
			return user.Select(u => new UserInfo
			{
				Id = u.Id,
				FirstName = u.FirstName,
				LastName = u.LastName,
				ScreenName = u.ScreenName,
				Photo = u.Photo100.ToString()
			});
		}
	}
}
