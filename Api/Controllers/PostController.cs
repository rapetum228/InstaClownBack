using Api.Exceptions;
using Api.Models;
using Api.Services;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly UserService _userService;

        public PostController(PostService postService, UserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost(PostCreateModel postCreateModel)
        {
            var author = await GetCurrentUser();
            await _postService.CreatePost(author, postCreateModel);
        }

        [HttpGet]
        public async Task<ICollection<PostModel>?> GetUserPosts(Guid userId)
        {
            var posts = await _postService.GetUserPostsAsync(userId);
            return posts;

        }

        [HttpGet]
        public async Task<FileResult> GetContent(Guid contentId)
        {
            var attach = await _postService.GetContent(contentId);
            return File(System.IO.File.ReadAllBytes(attach.FilePath), attach.MimeType);

        }

        private async Task<UserModel> GetCurrentUser()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                return await _userService.GetUser(userId);
            }
            else
                throw new TypeMismatchException("The entered Id does not match Guid!");
        }

        //private async Task<PostMetadataModel> GetPath(MetadataModel model)
        //{
        //    var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
        //    if (!tempFi.Exists)
        //        throw new Exception("file not found");
        //    else
        //    {
        //        var path = Path.Combine(Directory.GetCurrentDirectory(), "attaches", model.TempId.ToString());
        //        var destFi = new FileInfo(path);
        //        if (destFi.Directory != null && !destFi.Directory.Exists)
        //            destFi.Directory.Create();

        //        System.IO.File.Copy(tempFi.FullName, path, true);

        //        var postModel = (PostMetadataModel)model;
        //        postModel.FilePath = path;
        //        return postModel;
        //    }
        //}
    }
}
