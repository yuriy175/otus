namespace Dialogs.Core.Model.Interfaces
{
    public interface IDialogsService
    {
        Task<Message> CreateMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken);
        Task<IEnumerable<Message>> GetMessagesAsync(uint userId1, uint userId2, CancellationToken cancellationToken);
        Task<IEnumerable<int>> GetBuddyIdsAsync(uint userId, CancellationToken cancellationToken);
    }
}
