using Api.Configs;
using Api.Exceptions;
using Api.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Api.Services
{
    public class UserService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public UserService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task CreateUser(CreateUserModel model)
        {
            var dbUser = _mapper.Map<User>(model);
            await _context.Users.AddAsync(dbUser);
            await _context.SaveChangesAsync();
        }
        public async Task<List<UserModel>> GetUsers()
        {
            return await _context.Users.AsNoTracking().ProjectTo<UserModel>(_mapper.ConfigurationProvider).ToListAsync();
        }

        private async Task<User> GetUserById(Guid id)
        {
            var user = await _context.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new NotFoundException("User not found!");
            return user;
        }
        public async Task<UserModel> GetUser(Guid id)
        {
            var user = await GetUserById(id);

            return _mapper.Map<UserModel>(user);

        }

        public async Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath)
        {
            var user = await _context.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var avatar = new Avatar { Author = user, MimeType = meta.MimeType, FilePath = filePath, Name = meta.Name, Size = meta.Size };
                user.Avatar = avatar;

                await _context.SaveChangesAsync();
            }

        }

        public async Task<AttachModel> GetUserAvatar(Guid userId)
        {
            var user = await GetUserById(userId);
            var atach = _mapper.Map<AttachModel>(user.Avatar);
            return atach;
        }
    }
}
