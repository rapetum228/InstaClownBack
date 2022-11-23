using Api.Exceptions;
using Api.Models;
using Api.Services;
using Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly UserService _userService;
        private readonly CommentService _commentService;

        public CommentController(PostService postService, UserService userService, CommentService commentService, LinkGeneratorService links)
        {
            _postService = postService;
            _userService = userService;
            _commentService = commentService;
            links.LinkAvatarGenerator = x => Url.ControllerAction<AttachController>(nameof(AttachController.GetUserAvatar), new
            {
                userId = x.Id,
            });
        }

        [HttpPost]
        [Authorize]
        public async Task AddCommentToPost(CommentRequestModel commentRequest)
        {
            if (commentRequest.AuthorId == default)
            {
                var userId = GetCurrentUserId();
                commentRequest.AuthorId = userId;
            }
            await _commentService.AddCommentToPost(commentRequest);
        }

        [HttpPost]
        [Authorize]
        public async Task RemoveComment(CommentSimpleModel commentRequest)
        {
            if (commentRequest.UserId == default)
            {
                var userId = GetCurrentUserId();
                commentRequest.UserId = userId;
            }
            await _commentService.RemoveComment(commentRequest);
        }
        [HttpGet]
        //[Authorize]
        public async Task<List<CommentModel>> GetPostComments(Guid postId)
        {
            return await _commentService.GetPostComments(postId);

        }

        [HttpPost]
        [Authorize]
        public async Task AddCommentToComment(CommentToCommentRequestModel commentToCommentRequest)
        {
            if (commentToCommentRequest.AuthorId == default)
            {
                var userId = GetCurrentUserId();
                commentToCommentRequest.AuthorId = userId;

            }
            await _commentService.AddCommentToComment(commentToCommentRequest);
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
