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
    [Route("api/[controller]")]
    [ApiController]
    public class TrackerController : ControllerBase
    {
        private readonly ITracker _tracker;
        private readonly IUserInfoSource _userSource;
        private readonly IUserInfoRepository _userRepository;
        private readonly IUsersRepository _usersRepository;

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

        [HttpPost("addUsers")]
        public async Task<BaseResponse> AddUsers([FromBody]AddUsersRequest request)
        {
            var users = (await _usersRepository.GetUsersAsync()).ToList();
            var newUsers = request.Ids.Except(request.Ids.Intersect(users)).Distinct().ToList();

            foreach (var id in newUsers)
            {
                await _usersRepository.AddUserAsync(id);
            }

            _tracker.AddUsers(newUsers);

            return new BaseSuccessResponse();
        }

        [HttpGet("users")]
        public async Task<BaseResponse<IEnumerable<UserInfo>>> GetUsers()
        {
            var userIds = await _usersRepository.GetUsersAsync();
            var userInfos = await _userSource.GetUsersInfo(userIds);

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
                result = await _userRepository.ReadDataAsync(id, dateFrom, to.Value);
            }
            else
            {
                result = await _userRepository.ReadDataAsync(id, dateFrom, dateFrom.AddDays(1));
            }

            return new BaseSuccessResponse<UserOnlineData>
            {
                Data = result
            };
        }
    }
}
