using Api.Configs;
using Api.Exceptions;
using Api.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Common;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Http.Metadata;
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

        public async Task<List<UserSimpleModel>> GetUserSubscriptions(Guid userId)
        {
            var user = await _context.Users.Include(u => u.Subscriptions)
                                           !.ThenInclude(u=>u.Avatar)
                                           .Include(u => u.Avatar)
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == default)
            {
                throw new NotFoundException("User not found");
            }
            var subscriptions = _mapper.Map<List<UserSimpleModel>>(user.Subscriptions);
            return subscriptions;
        }

        public async Task<List<UserSimpleModel>> GetUserSubscribers(Guid userId)
        {
            var user = await _context.Users.Include(u => u.Subscribers)
                                           !.ThenInclude(u => u.Avatar)
                                           .Include(u => u.Avatar)
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == default)
            {
                throw new NotFoundException("User not found");
            }
            var subscriptions = _mapper.Map<List<UserSimpleModel>>(user.Subscriptions);
            return subscriptions;
        }

        public async Task SubscribeToUser(Guid userId, Guid subscriberId)
        {
            if (userId == subscriberId)
            {
                throw new Exception("You can't subscribe to yourself");
            }
            var user = await _context.Users.Include(u => u.Subscribers).FirstOrDefaultAsync(u=>u.Id==userId);
            var subscriber = await GetUserById(subscriberId);
            if (user == default)
            {
                throw new Exception("User not found");
            }
            if (user.Subscribers is null)
            {
                user.Subscribers = new List<User>();
            }
            if (user.Subscribers.Select(u => u.Id).Contains(subscriberId))
            {
                throw new Exception("You already subscribed on this user");
            }
            user.Subscribers.Add(subscriber);
            await _context.SaveChangesAsync();
        }

        public async Task UnsubscribeFromUser(Guid userId, Guid subscriberId)
        {
            var user = await _context.Users.Include(u => u.Subscribers).FirstOrDefaultAsync(u => u.Id == userId);
            var subscriber = await GetUserById(subscriberId);
            if (user == default)
            {
                throw new Exception("User not found");
            }
            
            if (user.Subscribers is null || !user.Subscribers.Select(u => u.Id).Contains(subscriberId))
            {
                throw new Exception("You are not already subscribed to this user");
            }
            user.Subscribers.Remove(subscriber);
            await _context.SaveChangesAsync();
        }

        public async Task AddAvatarToUser(Guid userId, MetadataModel meta, string filePath)
        {
            var user = await _context.Users.Include(x => x.Avatar).FirstOrDefaultAsync(x => x.Id == userId);
            if (user != null)
            {
                var avatar = new Avatar
                {
                    Author = user,
                    MimeType = meta.MimeType,
                    FilePath = filePath,
                    Name = meta.Name,
                    Size = meta.Size
                };
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
