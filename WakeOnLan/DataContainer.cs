using System.Text.Json.Serialization;

namespace WakeOnLan;

internal class DataContainer
{
    [JsonPropertyName("targets")]
    public Dictionary<string, Target>? Targets { get; set; }

}
