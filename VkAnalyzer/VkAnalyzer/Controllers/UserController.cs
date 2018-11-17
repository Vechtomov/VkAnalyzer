using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkAnalyzer.Models;
using VkAnalyzer.Interfaces;

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

        [HttpGet("find")]
        public async Task<BaseResponse<UsersResponse>> FindUser(string filter)
        {
            filter = filter?.Split('/').Last();
            var (users, count) = await userSource.SearchUsers(filter);
            if (users == null)
            {
                return new BaseSuccessResponse<UsersResponse>
                {
                    Data = new UsersResponse(),
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
    }
}
