using Api.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Api.Services
{
    public class CommentService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CommentService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task AddCommentToPost(CommentRequestModel commentRequest)
        {

            var user = await GetUser(commentRequest.AuthorId); //существует ли юзер
            var post = await GetPost(commentRequest.PostId); //существует ли пост
            var comment = _mapper.Map<Comment>(commentRequest);

            comment.UserId = user.Id;
            comment.PostId = post.Id;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

        }

        public async Task<List<CommentModel>> GetPostComments(Guid postId)
        {
            var post = await _context.Posts.AsNoTracking()
                                           .FirstOrDefaultAsync(p => p.Id == postId);
            if (post == default)
            {
                throw new Exception("Post not found");
            }
            var comments = await _context.Comments.AsNoTracking().Include(c => c.User).ThenInclude(u => u.Avatar)
                                                  .Include(c => c.Comments)
                                                  .Where(c => c.PostId == postId)
                                                  .ToListAsync();

            var commentModels = _mapper.Map<List<CommentModel>>(comments);
            return commentModels;
        }

        public async Task AddCommentToComment(CommentToCommentRequestModel commentRequest)
        {
            var commentForComment = await GetComment(commentRequest.CommentId); //есть ли коммент
            var commentator = await GetUser(commentRequest.AuthorId); //есть ли юзер

            if (commentForComment.Comments is null)
            {
                commentForComment.Comments = new List<Comment>();
            }
            var comment = _mapper.Map<Comment>(commentRequest);
            comment.UserId = commentator.Id;
            comment.PostId = commentForComment.PostId;
            commentForComment.Comments.Add(comment);

            await _context.SaveChangesAsync();
        }

        public async Task<Comment> GetComment(Guid commentId)
        {
            var comment = await _context.Comments.Include(c => c.Post).Include(c => c.Comments).FirstOrDefaultAsync(u => u.Id == commentId);
            if (comment == null)
            {
                throw new Exception("Comment not found");
            }
            return comment;
        }

        private async Task<Post> GetPost(Guid postId)
        {
            var post = await _context.Posts.AsNoTracking().Include(p => p.Author).FirstOrDefaultAsync(p => p.Id == postId);
            if (post == null)
            {
                throw new Exception("Post not found");
            }
            return post;
        }

        private async Task<User> GetUser(Guid? userId)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            return user;
        }
    }
}
