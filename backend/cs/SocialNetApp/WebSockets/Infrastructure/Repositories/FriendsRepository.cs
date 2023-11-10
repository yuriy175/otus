using Dapper;
using WebSockets.Infrastructure.Repositories.Interfaces;

namespace WebSockets.Infrastructure.Repositories
{
    public class FriendsRepository : IFriendsRepository
    {
        private DataContext _context;

        public FriendsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<int>> GetFriendIdsAsync(uint userId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT friend_id " +
                      "FROM public.friends " +
                      "WHERE user_id = @userId and \"isDeleted\" = false";

            return await connection.QueryAsync<int>(new CommandDefinition(
                sql,
                new { userId = (int)userId },
                cancellationToken: cancellationToken));
        }
    }
}
