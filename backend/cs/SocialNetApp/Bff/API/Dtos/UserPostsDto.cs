using System.ComponentModel.DataAnnotations;

namespace Bff.API.Dtos
{
    public readonly record struct UserPostsDto
    {
        public UserDto User { get; init; }
        public IEnumerable<PostDto> Posts { get; init; }
    }
}
