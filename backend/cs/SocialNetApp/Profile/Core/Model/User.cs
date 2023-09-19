using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SocialNetApp.Core.Model
{
    public readonly record  struct User
    {
        public uint Id { get; init; }
        public string Name { get; init; }
        public string Surname { get; init; }
        public byte Age { get; init; }
        public string Sex { get; init; }
        public string City { get; init; }
        public string Info { get; init; }
    }
}
