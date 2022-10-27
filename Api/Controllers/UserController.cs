using Api.Models;
using Api.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")] //in default "api/[controller]". Addition [action] views controller method names (CreateUser,GetUsers..)
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        //url with action: api/User/CreateUser
        //url without action: api/User
        [HttpPost]
        public async Task CreateUser(CreateUserModel model)=> await _userService.CreateUser(model);

        [HttpGet]
        public async Task<List<UserModel>> GetUsers() => await _userService.GetUsers();
        
    }
}
