using AutoMapper;
using Profile.Core.Model.Interfaces;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.Core.Model;

namespace Profile.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public Task<User> GetUserByIdAsync(uint userId) => _userRepository.GetUserByIdAsync(userId);

        public Task<IEnumerable<User>> GetUsersAsync() => _userRepository.GetUsersAsync();

        public Task<int> PutUserAsync(User user, string password) => _userRepository.PutUserAsync(user, password);

        public Task<IEnumerable<User>> GetUsersByNameAsync(string name, string surname) => _userRepository.GetUsersByNameAsync(name, surname);
    }
}
