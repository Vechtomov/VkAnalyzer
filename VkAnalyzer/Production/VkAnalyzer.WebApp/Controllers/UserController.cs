using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkAnalyzer.BE;
using VkAnalyzer.Interfaces;
using VkAnalyzer.WebApp.Models;

namespace VkAnalyzer.WebApp.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserInfoSource _userSource;
		public UserController(IUserInfoSource userInfoSource)
		{
			_userSource = userInfoSource;
		}

		[HttpGet("find")]
		public async Task<BaseResponse<UsersResponse>> FindUser(string filter)
		{
			filter = filter?.Split('/').Last();
			var (users, count) = await _userSource.SearchUsers(filter);
			if (users == null)
			{
				return new BaseSuccessResponse<UsersResponse>
				{
					Data = new UsersResponse()
				};
			}

			return new BaseSuccessResponse<UsersResponse>
			{
				Data = new UsersResponse
				{
					TotalCount = count,
					Users = users
				}
			};
		}

		[HttpGet("info")]
		public async Task<BaseResponse<UserInfo>> GetUserInfo(long id)
		{
			var user = (await _userSource.GetUsersInfo(new[] { id })).FirstOrDefault();

			if (user == null)
			{
				throw new BaseApiException("Пользователь не найден");
			}

			return new BaseSuccessResponse<UserInfo>
			{
				Data = user
			};
		}


		/// <summary />
		/// <returns></returns>
		[HttpGet("friends")]
		public async Task<BaseResponse<IEnumerable<UserInfo>>> GetFriends([FromQuery] GetFriendsRequest request)
		{
			return new BaseSuccessResponse<IEnumerable<UserInfo>>
			{
				Data = await _userSource.GetUserFriends(request.UserId)
			};
		}

	}
}
