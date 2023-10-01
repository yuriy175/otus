using SocialNetApp.Core.Model;

namespace Profile.Core.Model.Interfaces
{
    public interface ILoadDataService
    {
        Task<int> LoadCitiesAsync(CancellationToken cancellationToken);
        Task<int> LoadPostAsync(int[] userIds, CancellationToken cancellationToken);
        Task<int> LoadUsersAsync(CancellationToken cancellationToken);
    }
}
