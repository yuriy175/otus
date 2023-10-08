using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Profile.Core.Model;
using Profile.Core.Model.Interfaces;
using Profile.Core.Services;
using Profile.Infrastructure.Caches;
using Profile.Infrastructure.Repositories;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Profile.Core.Services
{
    public class AuthService : IAuthService
    {        
        private readonly IUserRepository _userRepository;
        private readonly ICacheService _cacheService;
        private readonly IPostsRepository _postsRepository;
        private readonly IFriendsRepository _friendsRepository;

        private readonly static string? _securityKey = Environment.GetEnvironmentVariable("SECURITY_KEY");
        private readonly static uint _cacheItemsCount = Convert.ToUInt32(Environment.GetEnvironmentVariable("CACHE_ITEMS_COUNT"));        

        public AuthService(
            IUserRepository userRepository,
            IPostsRepository postsRepository,
            IFriendsRepository friendsRepository,
            ICacheService cacheService)
        {
            _cacheService = cacheService;
            _userRepository = userRepository;
            _postsRepository = postsRepository;
            _friendsRepository = friendsRepository;
            if (_securityKey is null)
            {
                throw new ApplicationException("Пустой секьюрити ключ");
            }
        }

        public async Task<string> LoginAsync(uint userId, string password, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.CheckUserAsync(userId, password);

            var claims = new List<Claim> { new Claim(Constants.UserIdClaimType, userId.ToString()) };
            var jwt = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: new SigningCredentials(
                        GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));

            if (exists)
            {
                //var friendIds = await _friendsRepository.GetFriendIdsAsync(userId, cancellationToken);
                //var posts = await _postsRepository.GetLatestPostsAsync(userId, _cacheItemsCount, cancellationToken);
                //await _cacheService.WarmupCacheAsync(userId, posts);

                //userId = 1218274;
                //var posts2 = await _postsRepository.GetLatestPostsAsync(userId, _cacheItemsCount, cancellationToken);
                //await _cacheService.WarmupCacheAsync(userId, posts2);

                //userId = 1218275;
                //var posts3 = await _postsRepository.GetLatestPostsAsync(userId, _cacheItemsCount, cancellationToken);
                //await _cacheService.WarmupCacheAsync(userId, posts3);
            }

            return !exists ? string.Empty : new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey!));
        }
    }
}
