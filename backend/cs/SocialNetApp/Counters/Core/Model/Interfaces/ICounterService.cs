namespace Counters.Core.Model.Interfaces
{
    public interface ICounterService
    {
        Task<int> UpdateUnReadCounterByUserIdAsync(uint userId, int delta, CancellationToken cancellationToken);
        Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken);
    }
}
