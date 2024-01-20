using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;

namespace Dialogs.Core.Model
{
    public readonly record  struct Message
    {
        [JsonPropertyName("id")]
        public uint Id { get; init; }

        [JsonPropertyName("authorId")]
        public uint AuthorId { get; init; }

        [JsonPropertyName("userId")]
        public ulong UserId { get; init; }

        [JsonPropertyName("message")]
        public string Text { get; init; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; init; }
    }
}