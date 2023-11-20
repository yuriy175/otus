using System.ComponentModel.DataAnnotations;

namespace Posts.API.Dtos
{
    public readonly record  struct UpdatePostDto
    {
        [Required]
        public uint PostId { get; init; }
        [Required]
        public string Text { get; init; }
    }
}
