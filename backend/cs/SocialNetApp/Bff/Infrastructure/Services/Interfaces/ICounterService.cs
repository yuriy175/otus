using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface ICounterService
    {
        Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken);
    }
}
