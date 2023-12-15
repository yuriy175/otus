using Bff.API.Dtos;

namespace Bff.Infrastructure.gRpc.Services.Interfaces
{
    public interface IDialogService
    {
        Task<MessageDto> CreateMessageAsync(uint value, uint userId, string text, CancellationToken cancellationToken);
        Task<IEnumerable<MessageDto>> GetMessagesAsync(uint value, uint userId, CancellationToken cancellationToken);
    }
}
