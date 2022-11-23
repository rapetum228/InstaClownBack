﻿using Api.Exceptions;
using Api.Models;
using Api.Services;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")] 
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task CreateUser(CreateUserModel model)=> await _userService.CreateUser(model);

        [HttpGet]
        public async Task<List<UserModel>> GetUsers() => await _userService.GetUsers();

        [HttpGet]
        [Authorize]
        public async Task<UserModel> GetCurrentUser()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                return await _userService.GetUser(userId);
            }
            else
                throw new TypeMismatchException("The entered Id does not match Guid!");

        }

        [HttpPost]
        [Authorize]
        public async Task AddAvatarToUser(MetadataModel model)
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
                if (!tempFi.Exists)
                    throw new Exception("file not found");
                else
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "attaches", model.TempId.ToString());
                    var destFi = new FileInfo(path);
                    if (destFi.Directory != null && !destFi.Directory.Exists)
                        destFi.Directory.Create();

                    System.IO.File.Copy(tempFi.FullName, path, true);

                    await _userService.AddAvatarToUser(userId, model, path);
                }
            }
            else
                throw new Exception("you are not authorized");

        }

        [HttpPost]
        [Authorize]
        public async Task SubscribeToUser(Guid userId, Guid? subscriberId)
        {
            if (subscriberId == default)
            {
                var currentUser = await GetCurrentUser();
                subscriberId = currentUser.Id;
            }
            await _userService.SubscribeToUser(userId, subscriberId!.Value);
        }

        [HttpPost]
        [Authorize]
        public async Task UnsubscribeFromUser(Guid userId, Guid? subscriberId)
        {
            if (subscriberId == default)
            {
                var currentUser = await GetCurrentUser();
                subscriberId = currentUser.Id;
            }
            await _userService.UnsubscribeFromUser(userId, subscriberId!.Value);
        }

        [HttpGet]
        public async Task<List<UserSimpleModel>> GetUserSubscriptions(Guid userId)
        {
            return await _userService.GetUserSubscriptions(userId);
        }
        [HttpGet]
        public async Task<List<UserSimpleModel>> GetUserSubscribers(Guid userId)
        {
            return await _userService.GetUserSubscribers(userId);
        }

        [HttpGet]
        public async Task<FileResult> GetUserAvatar(Guid userId)
        {
            var attach = await _userService.GetUserAvatar(userId);

            return File(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType);
        }

        [HttpGet]
        public async Task<FileResult> DownloadAvatar(Guid userId)
        {
            var attach = await _userService.GetUserAvatar(userId);

            HttpContext.Response.ContentType = attach.MimeType;
            FileContentResult result = new FileContentResult(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType)
            {
                FileDownloadName = attach.Name
            };

            return result;
        }
    }
}
