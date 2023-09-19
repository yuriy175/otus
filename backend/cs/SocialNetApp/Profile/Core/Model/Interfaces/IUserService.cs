using SocialNetApp.Core.Model;

namespace Profile.Core.Model.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(uint userId);
        Task<int> PutUserAsync(User user, string password);
    }
}
