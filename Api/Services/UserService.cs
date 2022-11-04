using Api.Configs;
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
        private readonly AuthConfig _config; //конфигурации токена

        public UserService(IMapper mapper, DataContext context, IOptions<AuthConfig> config)
        {
            _mapper = mapper;
            _context = context;
            _config = config.Value;
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
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                throw new Exception("User not found");
            return user;
        }
        public async Task<UserModel> GetUser(Guid id)
        {
            var user = await GetUserById(id);

            return _mapper.Map<UserModel>(user);

        }
    }
}
