using Dapper;
using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Repositories.Interfaces
{
    public interface IFriendsRepository
    {
        Task<int> UpsertFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
        Task<int> DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetFriendIdsAsync(uint userId, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetSubscriberIdsAsync(uint userId, CancellationToken cancellationToken);
    }
}
