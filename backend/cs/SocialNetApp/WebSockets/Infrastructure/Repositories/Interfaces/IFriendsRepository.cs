namespace WebSockets.Infrastructure.Repositories.Interfaces
{
    public interface IFriendsRepository
    {
        Task<IEnumerable<int>> GetFriendIdsAsync(uint userId, CancellationToken cancellationToken);
    }
}
