using System.ComponentModel.DataAnnotations;

namespace Bff.API.Dtos
{
    public readonly record struct MeasureDto
    {
        [Required]
        public ulong Id { get; init; }
        [Required]
        public ulong DeviceId { get; init; }
        public ushort Type { get; init; }
        public DateTime Date { get; init; }
        public double Value { get; init; }
    }
}
