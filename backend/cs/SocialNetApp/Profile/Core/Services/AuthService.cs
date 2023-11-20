using Common.Core.Model;
using Microsoft.IdentityModel.Tokens;
using Profile.Core.Model.Interfaces;
using Profile.Infrastructure.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Profile.Core.Services
{
    public class AuthService : IAuthService
    {        
        private readonly IUserRepository _userRepository;

        private readonly static uint _cacheItemsCount = Convert.ToUInt32(Environment.GetEnvironmentVariable("CACHE_ITEMS_COUNT"));        

        public AuthService(
            IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
