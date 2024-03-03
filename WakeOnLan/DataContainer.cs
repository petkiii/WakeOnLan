using System.Text.Json.Serialization;

namespace WakeOnLan;

internal sealed record DataContainer
{
    [JsonPropertyName("targets")]
    public Dictionary<string, Target> Targets { get; set; } = new();
}