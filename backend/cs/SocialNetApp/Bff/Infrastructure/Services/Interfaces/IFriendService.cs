using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IFriendService
    {
        Task<UserDto> AddFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
        Task DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken);
        Task<IEnumerable<UserDto>> GetFriendsAsync(uint userId, CancellationToken cancellationToken);
    }
}
