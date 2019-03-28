using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;
using VkAnalyzer.Models;

namespace VkAnalyzer.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class TrackerController : ControllerBase
	{
		private readonly ITracker _tracker;
		private readonly IUserInfoSource _userSource;
		private readonly IUserInfoRepository _userInfoRepository;
		private readonly IUsersRepository _usersRepository;

		public TrackerController(ITracker tracker,
			IUserInfoSource userSource,
			IUserInfoRepository userRepository,
			IUsersRepository usersRepository)
		{
			_tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
			_userSource = userSource ?? throw new ArgumentNullException(nameof(userSource));
			_userInfoRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
			_usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
		}

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

		[HttpGet("users")]
		public async Task<BaseResponse<IEnumerable<UserInfo>>> GetUsers()
		{
			var userIds = await _usersRepository.GetUsersAsync();
			var userInfos = await _userSource.GetUsersInfo(userIds.Select(u => u.Id));

			return new BaseSuccessResponse<IEnumerable<UserInfo>>
			{
				Data = userInfos
			};
		}

		[HttpGet("getdata")]
		public async Task<BaseResponse<UserOnlineData>> GetUserOnlineData(long id, DateTime? from = null, DateTime? to = null)
		{
			var dateFrom = from ?? DateTime.Now;
			UserOnlineData result;

			if (to != null)
			{
				result = await _userInfoRepository.ReadDataAsync(id, dateFrom, to.Value);
			}
			else
			{
				result = await _userInfoRepository.ReadDataAsync(id, dateFrom, dateFrom.AddDays(1));
			}

			return new BaseSuccessResponse<UserOnlineData>
			{
				Data = result
			};
		}
	}
}
