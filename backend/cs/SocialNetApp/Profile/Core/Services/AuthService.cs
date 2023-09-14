using Profile.Core.Model.Interfaces;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;

namespace Profile.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> LoginAsync(uint userId, string password)
        {
            var exists = await _userRepository.CheckUserAsync(userId, password);

            return !exists ? string.Empty : Guid.NewGuid().ToString();
        }
    }
}
