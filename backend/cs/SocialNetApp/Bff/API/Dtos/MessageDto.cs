using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Bff.API.Dtos
{
    public readonly record struct MessageDto
    {
        [Required]
        [JsonPropertyName("id")]
        public uint Id { get; init; }

        [Required]
        [JsonPropertyName("authorId")]
        public uint AuthorId { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; init; }
    }
}
