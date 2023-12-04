using System.ComponentModel.DataAnnotations;

namespace Bff.API.Dtos
{
    public readonly record struct UserDto
    {
        [Required]
        public uint Id { get; init; }
        [Required]
        public string Name { get; init; }
        [Required]
        public string Surname { get; init; }
        [Required]
        public string Password { get; init; }
        public byte? Age { get; init; }
        public string Sex { get; init; }
        public string City { get; init; }
        public string Info { get; init; }
    }
}
