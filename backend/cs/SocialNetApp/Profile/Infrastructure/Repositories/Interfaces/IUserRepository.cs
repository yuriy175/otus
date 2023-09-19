using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(uint userId);
        Task<int> PutUserAsync(User user, string password);
        Task<bool> CheckUserAsync(uint userId, string password);
        Task<IEnumerable<User>> GetUsersByNameAsync(string name, string surname);
    }
}
