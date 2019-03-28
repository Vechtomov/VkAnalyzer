using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;
using WebApplication.Models;

namespace WebApplication.Controllers
{
	/// <summary />
	[Route("api/[controller]")]
	[ApiController]
	public class TrackerController : ControllerBase
	{
		private readonly ITracker _tracker;
		private readonly IUserInfoSource _userSource;
		private readonly IUserInfoRepository _userRepository;
		private readonly IUsersRepository _usersRepository;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="tracker"></param>
		/// <param name="userSource"></param>
		/// <param name="userRepository"></param>
		/// <param name="usersRepository"></param>
		public TrackerController(ITracker tracker,
			IUserInfoSource userSource,
			IUserInfoRepository userRepository,
			IUsersRepository usersRepository)
		{
			_tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
			_userSource = userSource ?? throw new ArgumentNullException(nameof(userSource));
			_userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			_usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
		}


		/// <summary />
		/// <param name="request"></param>
		/// <returns></returns>
		[HttpPost("addUsers")]
		public async Task<BaseResponse> AddUsers([FromBody]AddUsersRequest request)
		{
			var userIds = (await _usersRepository.GetUsersAsync()).ToList().Select(u => u.Id);
			var newUserIds = request.Ids
				.Except(request.Ids.Intersect(userIds))
				.Distinct()
				.ToList();

			var newUsers = await _userSource.GetUsersInfo(newUserIds);

			foreach (var user in newUsers)
			{
				await _usersRepository.AddUserAsync(new User
				{
					Id = user.Id,
					AddedDateTime = DateTime.Now,
					FirstName = user.FirstName,
					LastName = user.LastName
				});
			}

			_tracker.AddUsers(newUserIds);

			return new BaseSuccessResponse();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		[HttpGet("users")]
		public async Task<BaseResponse<IEnumerable<UserInfo>>> GetUsers()
		{
			var userIds = (await _usersRepository.GetUsersAsync()).ToList();
			IEnumerable<UserInfo> userInfos = new List<UserInfo>();

			if (userIds.Any())
			{
				userInfos = await _userSource.GetUsersInfo(userIds.Select(u => u.Id));
			}

			return new BaseSuccessResponse<IEnumerable<UserInfo>>
			{
				Data = userInfos
			};
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="id"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		[HttpGet("getdata")]
		public async Task<BaseResponse<UserOnlineData>> GetUserOnlineData(long id, long? from = null, long? to = null)
		{
			var dateFrom = from != null
				? DateTimeOffset.FromUnixTimeMilliseconds(from.Value)
					.DateTime.ToLocalTime()
				: DateTime.Now.AddDays(-1);

			var dateTo = to != null
				? DateTimeOffset.FromUnixTimeMilliseconds(to.Value)
					.DateTime.ToLocalTime()
				: DateTime.Now;

			var result = await _userRepository.ReadDataAsync(id, dateFrom, dateTo);

			var fixedInfos = result.OnlineInfos.ToList();

			for (var i = 0; i < fixedInfos.Count - 1; i++)
			{
				if (fixedInfos[i].Date < fixedInfos[i + 1].Date) continue;

				fixedInfos.RemoveAt(i);
				i--;
			}

			result.OnlineInfos = fixedInfos;

			return new BaseSuccessResponse<UserOnlineData>
			{
				Data = result
			};
		}
	}
}
