using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VkAnalyzer.Interfaces;
using VkAnalyzer.BE;
using VkAnalyzer.Models;
using VkAnalyzer.DbModels;

namespace VkAnalyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterrogatorController : ControllerBase
    {
        private readonly IInterrogator interrogator;
        private readonly IUserInfoSource userSource;
        private readonly IDataReader dataReader;

        public InterrogatorController(IInterrogator interrogator, IUserInfoSource userSource, IDataReader dataReader)
        {
            this.interrogator = interrogator;
            this.userSource = userSource;
            this.dataReader = dataReader;
        }

        [HttpPost("addUser")]
        public async Task<BaseResponse> AddUser(long id)
        {
            var usersContext = new UsersDbContext();

            var user = usersContext.Users.Where(u => u.UserId == id).FirstOrDefault();

            if (user == null)
            {
                await usersContext.Users.AddAsync(new User { UserId = id });
                await usersContext.SaveChangesAsync();
            }

            var result = interrogator.AddUsers(new[] { id });
            return new BaseResponse
            {
                Success = result
            };
        }

        [HttpGet("users")]
        public async Task<BaseResponse<IEnumerable<UserInfo>>> GetUsers()
        {
            var usersContext = new UsersDbContext();

            var userIds = usersContext.Users.Select(u => u.UserId).ToList();
            var userInfos = await userSource.GetUsersInfo(userIds);

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
                result = await dataReader.ReadDataAsync(id, dateFrom, to.Value);
            }
            else
            {
                result = await dataReader.ReadDataByDayAsync(id, dateFrom);
            }

            return new BaseSuccessResponse<UserOnlineData>
            {
                Data = result
            };
        }
    }
}
