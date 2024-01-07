namespace Posts.Core.Model.Interfaces
{
    public interface IFriendsService
    {
        Task<int> UpsertFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
        Task<int> DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
    }
}
