using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;

namespace Posts.Core.Model
{
    public readonly record  struct Post
    {
        [JsonPropertyName("id")]
        public uint Id { get; init; }

        [JsonPropertyName("authorId")]
        public uint AuthorId { get; init; }

        [JsonPropertyName("message")]
        public string Message { get; init; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; init; }
    }
}
