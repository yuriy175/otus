using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SocialNetApp.Core.Model
{
    public readonly record  struct Post
    {
        public uint Id { get; init; }
        public uint AuthorId { get; init; }
        public string Message { get; init; }
        public DateTime? Created { get; init; }
    }
}
