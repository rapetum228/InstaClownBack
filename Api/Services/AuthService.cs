using Api.Configs;
using Api.Models;
using AutoMapper;
using Common;
using DAL.Entities;
using DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Api.Exceptions;

namespace Api.Services
{
    public class AuthService
    {
        private readonly DataContext _context;
        private readonly AuthConfig _config; //конфигурации токена

        public AuthService(DataContext context, IOptions<AuthConfig> config)
        {
            _context = context;
            _config = config.Value;
        }

        private async Task<User> GetUserByCredention(string login, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email.ToLower() == login.ToLower());
            if (user == null)
                throw new NotFoundException("User not found!");

            if (!PasswordHash.ValidatePassword(password, user.PasswordHash))
                throw new Exception("Okay give me correct p[ass]word"); //Password is incorrect

            return user;
        }

        private TokenModel GenerateTokens(UserSession userSession)
        {
            var dtNow = DateTime.Now;

            if (userSession.User is null)
            {
                throw new Exception("Your little fucking games");
            }

            //генерация объекта jwt для первого входа
            var jwt = new JwtSecurityToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                notBefore: dtNow, //начало действия токена
                claims:
                new Claim[] { //клаймы юзера
                            new Claim(ClaimsIdentity.DefaultNameClaimType, userSession.User.Name),
                            new Claim("sessionId", userSession.Id.ToString()),
                            new Claim("id", userSession.User.Id.ToString()),
                            },
                expires: dtNow.AddMinutes(_config.LifeTime), //указание времени жизни
                signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256) //указание алгоритма подписи
                );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt); //подписание токена (будет зашифрованная подписанная строка, содерж токен)

            var refreshJwt = new JwtSecurityToken(
                //без издателя и аудиенции ,чтобы им нельзя было авторизоваться на входе
                notBefore: dtNow,
                claims: new Claim[] {
                new Claim("refreshToken", userSession.RefreshToken.ToString()),
                },
                expires: DateTime.Now.AddHours(_config.LifeTime),
                signingCredentials: new SigningCredentials(_config.SymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)
                );
            var encodedRefreshJwt = new JwtSecurityTokenHandler().WriteToken(refreshJwt);

            return new TokenModel(encodedJwt, encodedRefreshJwt);

        }
        public async Task<TokenModel> GetToken(string login, string password)
        {
            var user = await GetUserByCredention(login, password);
            var session = await _context.UserSessions.AddAsync(new UserSession
            {
                User = user,
                RefreshToken = Guid.NewGuid(),
                Created = DateTime.UtcNow,
                Id = Guid.NewGuid()
            });
            await _context.SaveChangesAsync();
            return GenerateTokens(session.Entity);

        }

        public async Task<UserSession> GetSessionById(Guid id)
        {
            var session = await _context.UserSessions.FirstOrDefaultAsync(x => x.Id == id);
            if (session == null)
            {
                throw new Exception("Session is not found");
            }
            return session;
        }

        private async Task<UserSession> GetSessionByRefreshToken(Guid id)
        {
            var session = await _context.UserSessions.Include(x => x.User).FirstOrDefaultAsync(x => x.RefreshToken == id);
            if (session == null)
            {
                throw new Exception("session is not found");
            }
            return session;
        }

        public async Task<TokenModel> GetTokenByRefreshToken(string refreshToken)
        {
            var validParams = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                IssuerSigningKey = _config.SymmetricSecurityKey()
            };
            var principal = new JwtSecurityTokenHandler().ValidateToken(refreshToken, validParams, out var securityToken);
            //securityToken - refreshToken из строки сконвертированный в объект

            if (securityToken is not JwtSecurityToken jwtToken //если ошибка валидации токена
                || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)) //или неверный алгоритм (не тот которым кодировали)
            {
                throw new SecurityTokenException("Invalid token");
            }

            if (principal.Claims.FirstOrDefault(x => x.Type == "refreshToken")?.Value is String refreshTokenString
                && Guid.TryParse(refreshTokenString, out var newRefreshToken))
            {
                var session = await GetSessionByRefreshToken(newRefreshToken);
                if (!session.IsActive)
                {
                    throw new Exception("session is not active");
                }

                session.RefreshToken = Guid.NewGuid();
                await _context.SaveChangesAsync();

                return GenerateTokens(session);
            }
            else
            {
                throw new SecurityTokenException("Invalid token");
            }
        }
    }
}
