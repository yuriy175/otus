using System.ComponentModel.DataAnnotations;

namespace Bff.API.Dtos
{
    public readonly record struct UserMessagesDto
    {
        public UserDto User { get; init; }
        public IEnumerable<MessageDto> Messages { get; init; }
    }
}
