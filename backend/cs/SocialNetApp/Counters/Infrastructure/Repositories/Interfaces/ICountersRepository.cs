using Dapper;

namespace Counters.Infrastructure.Repositories.Interfaces
{
    public interface ICountersRepository
    {
        Task<int> UpdateUnReadCounterByUserIdAsync(uint userId, int delta, CancellationToken cancellationToken);
        Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken);
    }
}
