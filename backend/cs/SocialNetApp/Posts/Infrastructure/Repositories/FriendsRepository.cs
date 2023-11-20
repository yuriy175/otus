using Dapper;
using Posts.Infrastructure.Repositories.Interfaces;

namespace Posts.Infrastructure.Repositories
{
    public class FriendsRepository : IFriendsRepository
    {
        private DataContext _context;

        public FriendsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> UpsertFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO public.friends(user_id, friend_id, \"isDeleted\") VALUES(@userId, @friendId, false) " +
                      "ON CONFLICT(user_id, friend_id) DO UPDATE SET \"isDeleted\" = false";

            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new { userId = (int)userId, friendId = (int)friendId },
                cancellationToken: cancellationToken));
        }

        public async Task<int> DeleteFriendAsync(uint userId, uint friendId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE public.friends SET \"isDeleted\"=true " +
                      "WHERE user_id = @userId and friend_id = @friendId";

            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new { userId = (int)userId, friendId = (int)friendId },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<int>> GetFriendIdsAsync(uint userId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT friend_id " +
                      "FROM public.friends " +
                      "WHERE user_id = @userId and \"isDeleted\" = false";

            return await connection.QueryAsync<int>(new CommandDefinition(
                sql,
                new { userId = (int)userId },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<int>> GetSubscriberIdsAsync(uint userId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT user_id " +
                      "FROM public.friends "+
                      "WHERE friend_id = @userId and \"isDeleted\" = false";

            return await connection.QueryAsync<int>(new CommandDefinition(
                sql,
                new { userId = (int)userId },
                cancellationToken: cancellationToken));
        }
    }
}
