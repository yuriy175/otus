using Dapper;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualBasic;
using Profile.Infrastructure.Repositories.Interfaces;
using SocialNetApp.API.Daos;
using SocialNetApp.Core.Model;
using System.Collections.Generic;
using System.Text;

namespace Profile.Infrastructure.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private DataContext _context;

        public PostsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<int> AddPostAsync(uint userId, string text, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO public.posts(user_id, message, \"isDeleted\") VALUES(@userId, @text, false)";

            return await connection.ExecuteAsync(new CommandDefinition(
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
            using var connection = _context.CreateConnection();
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
            using var connection = _context.CreateConnection();
            var sql = "SELECT id, user_id as authorId, message, created " +
                      "FROM public.posts " +
                      "WHERE \"isDeleted\"!=true " +
                      "LIMIT @limit OFFSET @offset";

            return await connection.QueryAsync<Post>(new CommandDefinition(
                sql,
                new { offset = (int)offset, limit = (int)limit },
                cancellationToken: cancellationToken));
        }
    }
}
