using Api.Exceptions;
using Api.Models;
using Api.Services;
using Common.Extensions;
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

        public PostController(PostService postService, UserService userService, LinkGeneratorService links)
        {
            _postService = postService;
            links.LinkContentGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetPostContent), new
            {
                postContentId = x.Id,
            });
            links.LinkAvatarGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task CreatePost(CreatePostRequest postRequest)
        {
            if (postRequest.AuthorId == default)
            {
                var authorId = GetCurrentUserId();
                postRequest.AuthorId = authorId;
            }
            await _postService.CreatePost(postRequest);
        }

        [HttpGet]
        public async Task<ICollection<PostModel>?> GetUserPosts(Guid userId)
        {
            var posts = await _postService.GetUserPostsAsync(userId);
            return posts;

        }

        [HttpPost]
        [Authorize]
        public async Task LikePost(LikeRequest likeRequest)
        {
            if (likeRequest.UserId == default)
            {
                likeRequest.UserId = GetCurrentUserId();
            }
            await _postService.LikePost(likeRequest);
        }

        [HttpPost]
        [Authorize]
        public async Task DislikePost(LikeRequest likeRequest)
        {
            if (likeRequest.UserId == default)
            {
                likeRequest.UserId = GetCurrentUserId();
            }
            await _postService.DislikePost(likeRequest);
        }

        private Guid GetCurrentUserId()
        {
            var userIdString = User.Claims.FirstOrDefault(x => x.Type == "id")?.Value;
            if (Guid.TryParse(userIdString, out var userId))
            {
                return userId;
            }
            else
                throw new TypeMismatchException("The entered Id does not match Guid!");
        }


    }
}
