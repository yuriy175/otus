using Profile.Core.Model;
using System.ComponentModel.DataAnnotations;

namespace SocialNetApp.API.Daos
{
    public readonly record  struct NewPostDao
    {
        public int UserId { get; init; }
        public string Message { get; init; }
    }
}
