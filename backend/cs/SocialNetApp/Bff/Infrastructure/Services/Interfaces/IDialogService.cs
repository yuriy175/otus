using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IDialogService
    {
        Task<MessageDto> CreateMessageAsync(uint authorId, uint userId, string text, CancellationToken cancellationToken);
        Task<UserMessagesDto> GetMessagesAsync(uint authorId, uint userId, CancellationToken cancellationToken);
    }
}
