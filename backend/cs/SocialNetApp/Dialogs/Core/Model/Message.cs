using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Serialization;

namespace Dialogs.Core.Model
{
    public readonly record  struct Message
    {
        [JsonPropertyName("userId1")]
        public ulong UserId1 { get; init; }

        [JsonPropertyName("userId2")]
        public ulong UserId2 { get; init; }

        [JsonPropertyName("authorId")]
        public uint AuthorId { get; init; }

        [JsonPropertyName("message")]
        public string Text { get; init; }

        [JsonPropertyName("created")]
        public DateTime? Created { get; init; }
    }
}