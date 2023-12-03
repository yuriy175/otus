using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IUserService
    {
        Task<LoggedinUserDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
    }
}
