﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VkAnalyzer.Interfaces;
using WebApplication.Models;

namespace WebApplication.Controllers
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