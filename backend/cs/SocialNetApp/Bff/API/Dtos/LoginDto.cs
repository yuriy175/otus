namespace Bff.API.Dtos
{
    public readonly record  struct LoginDto
    {
        public uint Id { get; init; }
	    public string Password { get; init; }
    }
}
