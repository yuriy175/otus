using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.IdentityModel.Tokens;
using Profile.Core.Model;
using Profile.Core.Model.Interfaces;
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
        private readonly static string? _securityKey = Environment.GetEnvironmentVariable("SECURITY_KEY");

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            if(_securityKey is null)
            {
                throw new ApplicationException("Пустой секьюрити ключ");
            }
        }

        public async Task<string> LoginAsync(uint userId, string password)
        {
            var exists = await _userRepository.CheckUserAsync(userId, password);

            var claims = new List<Claim> { new Claim(Constants.UserIdClaimType, userId.ToString()) };
            var jwt = new JwtSecurityToken(
                    claims: claims,
                    signingCredentials: new SigningCredentials(
                        GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256));

            return !exists ? string.Empty : new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_securityKey!));
    }
}
