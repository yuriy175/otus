using System.ComponentModel.DataAnnotations;

namespace Bff.API.Dtos
{
    public readonly record struct LoggedinUserDto
    {
        public UserDto User { get; init; }
        public string Token { get; init; }
    }
}
