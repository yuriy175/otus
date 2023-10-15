using SocialNetApp.API.Daos;
using SocialNetApp.Core.Model;
using System.Threading;

namespace Profile.Infrastructure.Repositories.Interfaces
{
    public interface ILoadDataRepository
    {
        Task<int> LoadCitiesAsync(IEnumerable<string> cityNames, CancellationToken cancellationToken);
        Task<int> LoadPostsAsync(IEnumerable<NewPostDao> posts, CancellationToken cancellationToken);
        Task<int> LoadUsersAsync(IEnumerable<NewUserDao> users, CancellationToken cancellationToken);
    }
}
