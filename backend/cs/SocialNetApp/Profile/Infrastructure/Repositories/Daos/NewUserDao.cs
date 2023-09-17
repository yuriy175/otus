using Profile.Core.Model;
using System.ComponentModel.DataAnnotations;

namespace SocialNetApp.API.Daos
{
    public readonly record  struct NewUserDao
    {
        public string Name { get; init; }
        public string Surname { get; init; }
        public string Password { get; init; }
        public byte Age { get; init; }
        public string? Sex { get; init; }
        public string? City { get; init; }
        public string? Info { get; init; }
    }
}
