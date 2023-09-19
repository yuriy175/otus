using SocialNetApp.API.Daos;
using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Repositories.Interfaces
{
    public interface ILoadDataRepository
    {
        Task<int> LoadCitiesAsync(IEnumerable<string> cityNames);
        Task<int> LoadUsersAsync(IEnumerable<NewUserDao> cityNames);
    }
}
