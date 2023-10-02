using Dapper;
using SocialNetApp.Core.Model;

namespace Profile.Infrastructure.Repositories.Interfaces
{
    public interface IFriendsRepository
    {
        Task<int> UpsertFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
        Task<int> DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
    }
}
