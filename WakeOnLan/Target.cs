using System.Net.NetworkInformation;
using System.Text.Json.Serialization;


namespace WakeOnLan;

internal sealed record Target
{
    [JsonPropertyName("normalizedName")]
    public required string NormalizedName { get; init; }

    [JsonPropertyName("name")]
    public required string Name { get; init; }

    [JsonPropertyName("host")]
    public required string Host { get; init; }

    [JsonPropertyName("port")]
    public required int Port { get; init; }

    [JsonPropertyName("macAddress")]
    [JsonConverter(typeof(PhysicalAddressJsonConverter))]
    public required PhysicalAddress MacAddress { get; init; }
}