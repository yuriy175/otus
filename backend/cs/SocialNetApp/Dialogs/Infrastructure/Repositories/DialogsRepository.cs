using Dapper;
using Dialogs.Core.Model;
using Dialogs.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Hosting;
using Microsoft.VisualBasic;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Dialogs.Infrastructure.Repositories
{
    public class DialogsRepository : IDialogsRepository
    {
        private DataContext _context;

        public DialogsRepository(DataContext context)
        {
            _context = context;
        }

        public async Task<Message> AddMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "INSERT INTO public.dialogs(author_id, user_id, message) " +
                      "VALUES (@authorId, @userId, @text) " +
                      "RETURNING author_id as authorId, user_id as userId, message as text, created";                      

            return await connection.QueryFirstAsync<Message>(new CommandDefinition(
                sql,
                new { authorId = (int)authorId, userId = (int)userId, text },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(uint userId1, uint userId2, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT author_id as authorId, user_id as userId, message as text, created " +
                      "FROM public.dialogs "+
                      "where author_id = @userId1 and user_id = @userId2 " +
                      "UNION ALL "+
                      "SELECT author_id as authorId, user_id as userId, message as text, created " +
                      "FROM public.dialogs "+
                      "where author_id = @userId2 and user_id = @userId1 " +
                      "ORDER BY created DESC;";

            return await connection.QueryAsync<Message>(new CommandDefinition(
                sql,
                new { userId1 = (int)userId1, userId2 = (int)userId2 },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<int>> GetBuddyIdsAsync(uint userId, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateReadConnection();
            var sql = "SELECT DISTINCT * FROM " +
                       "(SELECT author_id " +
                       "FROM public.dialogs " +
                       "WHERE user_id = @userId " +
                       "UNION ALL " +
                       "SELECT user_id " +
                       "FROM public.dialogs " +
                       "WHERE author_id = @userId) x";

            return await connection.QueryAsync<int>(new CommandDefinition(
                sql,
                new { userId = (int)userId },
                cancellationToken: cancellationToken));
        }
    }
}
