using System.Text.Json.Serialization;

namespace Measure.Core.Model
{
    public readonly record struct Measure
    {
        [JsonPropertyName("id")]
        public ulong Id { get; init; }

        [JsonPropertyName("deviceId")]
        public ulong DeviceId { get; init; }

        [JsonPropertyName("type")]
        public ushort Type { get; init; }

        [JsonPropertyName("date")]
        public DateTime Date { get; init; }

        [JsonPropertyName("value")]
        public double Value { get; init; }
    }
}
