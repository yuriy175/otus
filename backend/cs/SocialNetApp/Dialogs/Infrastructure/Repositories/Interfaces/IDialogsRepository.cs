using Dapper;
using Dialogs.Core.Model;

namespace Dialogs.Infrastructure.Repositories.Interfaces
{
    public interface IDialogsRepository
    {
        Task<Message> AddMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken);
        Task<IEnumerable<Message>> GetMessagesAsync(uint userId1, uint userId2, CancellationToken cancellationToken);
    }
}
