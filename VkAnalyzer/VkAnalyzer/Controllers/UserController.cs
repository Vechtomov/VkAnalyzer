using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkAnalyzer.BL;
using VkAnalyzer.Models;

namespace VkAnalyzer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserInfoSource userSource;
        public UserController(IUserInfoSource userInfoSource)
        {
            userSource = userInfoSource;
        }

        [HttpGet]
        public async Task<UsersResponse> Users(string filter)
        {
            filter = filter?.Split('/').Last();
            var (users, count) = await userSource.SearchUsers(filter);
            if (users == null)
            {
                return new UsersResponse
                {
                    TotalCount = 0,
                };
            }

            return new UsersResponse
            {
                TotalCount = count,
                Users = users
            };
        }
    }
}
