﻿namespace Bff.API.Dtos
{
    public record SearchUserDto
    {
        public string Name { get; init; } = default!;
        public string Surname { get; init; } = default!;
    }
}
