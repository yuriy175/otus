using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetUsersByNameAsync(string name, string surname);
        Task<LoggedinUserDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken);
        Task<UserDto> PutUserAsync(NewUserDto dto, string password);
    }
}
