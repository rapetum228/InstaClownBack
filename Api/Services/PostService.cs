using Api.Models;
using AutoMapper;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace Api.Services
{
    public class PostService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public PostService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreatePost(CreatePostRequest postRequest)
        {
            var user = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == postRequest.AuthorId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var createPostModel = _mapper.Map<CreatePostModel>(postRequest);

            createPostModel.Contents.ForEach(x =>
            {
                MoveAttach(x, createPostModel.AuthorId);
            });

            var dbModel = _mapper.Map<Post>(createPostModel);
            await _context.Posts.AddAsync(dbModel);
            await _context.SaveChangesAsync();
        }

        public async Task<ICollection<PostModel>?> GetUserPostsAsync(Guid userId)
        {
            var posts = await _context.Posts.Include(p => p.Attachments)//.ThenInclude(a=>a.Author)
                                      .Include(p=>p.Author)
                                      .ThenInclude(x => x.Avatar)
                                      .Where(p=>p.AuthorId == userId)
                                      .OrderByDescending(x => x.DateTimeCreation)
                                      .AsNoTracking().ToListAsync();

            return _mapper.Map<List<PostModel>>(posts);

        }

        public async Task<AttachModel> GetPostContent(Guid contentId)
        {
            var content = await _context.Attaches.FirstOrDefaultAsync(a=>a.Id == contentId);
            return _mapper.Map<AttachModel>(content);
        } 

        private void MoveAttach(MetadataLinkModel model, Guid authorId)
        {
            model.AuthorId = authorId;
            model.FilePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "attaches",
               model.TempId.ToString());

            var tempFi = new FileInfo(Path.Combine(Path.GetTempPath(), model.TempId.ToString()));
            if (tempFi.Exists)
            {
                var destFi = new FileInfo(model.FilePath);
                if (destFi.Directory != null && !destFi.Directory.Exists)
                    destFi.Directory.Create();

                File.Move(tempFi.FullName, model.FilePath, true);
            }
        }


    }
}
