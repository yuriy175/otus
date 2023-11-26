using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace SocialNetApp.Core.Model
{
    public readonly record  struct User
    {
        [Required]
        public uint Id { get; init; }
        [Required]
        public string Name { get; init; }
        [Required]
        public string Surname { get; init; }
        public byte Age { get; init; }
        public string Sex { get; init; }
        public string City { get; init; }
        public string Info { get; init; }
    }
}
