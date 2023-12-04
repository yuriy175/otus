using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IFriendService
    {
        Task<IEnumerable<UserDto>> GetFriendsAsync(uint userId, CancellationToken cancellationToken);
    }
}
