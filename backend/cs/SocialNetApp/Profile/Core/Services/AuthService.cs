using Common.Core.Model;
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
        }

        public async Task<string> LoginAsync(uint userId, string password, CancellationToken cancellationToken)
        {
            var exists = await _userRepository.CheckUserAsync(userId, password);

            var claims = new List<Claim> { new Claim(Constants.UserIdClaimType, userId.ToString()) };
            var jwt = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: new SigningCredentials(
                        AuthUtils.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));

            return !exists ? string.Empty : new JwtSecurityTokenHandler().WriteToken(jwt);
        }
    }
}
