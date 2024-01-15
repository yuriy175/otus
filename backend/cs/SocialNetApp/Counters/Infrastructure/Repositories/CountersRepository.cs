using Counters.Infrastructure.Repositories.Interfaces;
using Dapper;

namespace Counters.Infrastructure.Repositories
{
    public class CountersRepository : ICountersRepository
    {
        private DataContext _context;

        public CountersRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> UpdateUnReadCounterByUserIdAsync(uint userId, int delta, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO counters (user_id, unread_count) " +
                        "VALUES(@userId,@delta) " +
                        "ON CONFLICT (user_id) " +
                        "DO " +
                           "UPDATE SET unread_count = ( " +
                               "SELECT unread_count + @delta  " +
                               "FROM counters " +
                               "WHERE user_id = @userId " +
                           ")";

            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new { userId = (int)userId, delta = delta },
                cancellationToken: cancellationToken));
        }

        public async Task<uint> GetUnReadCounterByUserIdAsync(uint userId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT unread_count FROM public.counters WHERE user_id = @userId;";

            return await connection.QueryFirstOrDefaultAsync<uint>(new CommandDefinition(
                sql,
                new { userId = (int)userId },
                cancellationToken: cancellationToken));
        }
    }
}
