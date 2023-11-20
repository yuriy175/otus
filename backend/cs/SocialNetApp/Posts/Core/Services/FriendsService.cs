using Posts.Core.Model.Interfaces;
using Posts.Infrastructure.Repositories.Interfaces;

namespace Posts.Core.Services
{
    public class FriendsService : IFriendsService
    {        
        private readonly IFriendsRepository _friendsRepository;

        public FriendsService(IFriendsRepository friendsRepository)
        {
            _friendsRepository = friendsRepository;
        }

        public Task<int> UpsertFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken) =>
            _friendsRepository.UpsertFriendAsync(userId, friendId, cancellationToken);

        public Task<int> DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken) =>
            _friendsRepository.DeleteFriendAsync(userId, friendId, cancellationToken);
    }
}
