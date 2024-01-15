using Counters.Core.Model.Interfaces;
using Counters.Infrastructure.Repositories.Interfaces;

namespace Counters.Core.Services
{
    public class CounterService : ICounterService
    {
        private readonly ICountersRepository _counterRepository;

        public CounterService(ICountersRepository counterRepository)
        {
            _counterRepository = counterRepository;
        }

        public Task<int> UpdateUnReadCounterByUserIdAsync(uint userId, int delta, CancellationToken cancellationToken) =>
            _counterRepository.UpdateUnReadCounterByUserIdAsync(userId, delta, cancellationToken);

        public Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken) =>
            _counterRepository.GetUnReadCounterByUserIdAsync(userId, cancellationToken);
    }
}
