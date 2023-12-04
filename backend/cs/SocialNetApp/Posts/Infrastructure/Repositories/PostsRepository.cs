using Dapper;
using Posts.Core.Model;
using Posts.Infrastructure.Repositories.Interfaces;

namespace Posts.Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private DataContext _context;

        public PostsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Post> AddPostAsync(uint userId, string text, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO public.posts(user_id, message, \"isDeleted\") VALUES(@userId, @text, false) "+
                      "RETURNING id, user_id as authorId, message, created";

            return await connection.QueryFirstAsync<Post>(new CommandDefinition(
                sql,
                new { userId = (int)userId, text },
                cancellationToken: cancellationToken));
        }

        public async Task<int> UpdatePostAsync(uint userId, uint postId, string text, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE public.posts SET message=@text " +
                      "WHERE id = @postId";

            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new { postId = (int)postId, text },
                cancellationToken: cancellationToken));
        }

        public async Task<int> DeletePostAsync(uint userId, uint postId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "UPDATE public.posts SET \"isDeleted\"=true " +
                      "WHERE id = @postId";

            return await connection.ExecuteAsync(new CommandDefinition(
                sql,
                new { postId = (int)postId },
                cancellationToken: cancellationToken));
        }

        public async Task<Post> GetPostAsync(uint userId, uint postId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT id, user_id as authorId, message, created " +
                      "FROM public.posts "+
                      "WHERE id = @postId and \"isDeleted\"!=true";

            return await connection.QuerySingleAsync<Post>(new CommandDefinition(
                sql,
                new { postId = (int)postId },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<Post>> GetPostsAsync(uint userId, uint offset, uint limit, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT id, user_id as authorId, message, created " +
                      "FROM public.posts " +
                      "WHERE \"isDeleted\"!=true " +
                      "LIMIT @limit OFFSET @offset";

            return await connection.QueryAsync<Post>(new CommandDefinition(
                sql,
                new { offset = (int)offset, limit = (int)limit },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<Post>> GetLatestPostsAsync(uint userId, uint limit, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT id, user_id as authorId, message, created " +
                      "FROM public.posts " +
                      "WHERE user_id = @userId and \"isDeleted\"!=true " +
                      "ORDER BY id DESC LIMIT @limit";

            return await connection.QueryAsync<Post>(new CommandDefinition(
                sql,
                new { userId = (int)userId, limit = (int)limit },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<Post>> GetLatestFriendsPostsAsync(uint userId, uint limit, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT id, user_id as authorId, message, created " +
                      "FROM public.posts p " +
                      "WHERE user_id IN( " +
                        "SELECT friend_id " +
                        "FROM public.friends f " +
                        "WHERE f.user_id = @userId and f.\"isDeleted\" = false " +
                       ") and p.\"isDeleted\" = false " +
                       "ORDER BY id DESC LIMIT @limit";

            return await connection.QueryAsync<Post>(new CommandDefinition(
                sql,
                new { userId = (int)userId, limit = (int)limit },
                cancellationToken: cancellationToken));
        }
    }
}
