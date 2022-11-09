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

        public async Task CreatePost(UserModel userModel, PostCreateModel postCreateModel)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == userModel.Id);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var post = _mapper.Map<Post>(postCreateModel);
            post.Author = user;
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            foreach (var meta in postCreateModel.PostMetas)
            {
                await AddAttachToPost(meta, post);
            }
        }

        public async Task<ICollection<PostModel>?> GetUserPostsAsync(Guid userId)
        {
            var posts = _context.Posts.Include(p => p.Attachments).Include(p=>p.Author).Where(p=>p.Author.Id == userId);

            return _mapper.Map<List<PostModel>>(posts);

            //var user = await _context.Users.Include(x => x.Posts).FirstOrDefaultAsync(x => x.Id == userId);
        }

        public async Task<AttachModel> GetContent(Guid contentId)
        {
            var content = await _context.Attaches.FirstOrDefaultAsync(a=>a.Id == contentId);
            return _mapper.Map<AttachModel>(content);
        } 

        private async Task AddAttachToPost(MetadataModel meta, Post post)
        {
            
                var postAttach = new PostAttach
                {
                    Author = post.Author,
                    MimeType = meta.MimeType,
                    FilePath = GetPath(meta),
                    Name = meta.Name,
                    Size = meta.Size
                };
            postAttach.Post = post;

            await _context.PostAttaches.AddAsync(postAttach);
            await _context.SaveChangesAsync();

        } 

        private string GetPath(MetadataModel model)
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

                return path;    
            }
        }
    }
}
