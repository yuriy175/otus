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
            var sql = "INSERT INTO public.dialogs(user_id_1, user_id_2, author_id, message) "+
                      "VALUES (@authorId, @userId, @authorId, @text) " +
                      "RETURNING user_id_1, user_id_2, author_id, message, created";                      

            return await connection.QueryFirstAsync<Message>(new CommandDefinition(
                sql,
                new { authorId = (int)authorId, userId = (int)userId, text },
                cancellationToken: cancellationToken));
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync(uint userId1, uint userId2, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();
            var sql = "SELECT user_id_1 as userId1, user_id_2 as userId2, author_id as authorId, message as text, created " +
                      "FROM public.dialogs "+
                      "where user_id_1 = @userId1 and user_id_2 = @userId2 " +
                      "UNION ALL "+
                      "SELECT user_id_1 as userId1, user_id_2 as userId2, author_id as authorId, message as text, created " +
                      "FROM public.dialogs "+
                      "where user_id_1 = @userId2 and user_id_2 = @userId1;";

            return await connection.QueryAsync<Message>(new CommandDefinition(
                sql,
                new { userId1 = (int)userId1, userId2 = (int)userId2 },
                cancellationToken: cancellationToken));
        }
    }
}
