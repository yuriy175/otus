using System.ComponentModel.DataAnnotations;

namespace Bff.API.Dtos
{
    public readonly record struct UserPostsDto
    {
        public IEnumerable<UserDto> Authors { get; init; }
        public IEnumerable<PostDto> Posts { get; init; }
    }
}
