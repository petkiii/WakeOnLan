using System.Net.NetworkInformation;
using System.Text.Json.Serialization;

// ReSharper disable PropertyCanBeMadeInitOnly.Global

namespace WakeOnLan;

internal class Target : ITarget
{
    [JsonPropertyName("normalizedName")]
    public string NormalizedName { get; set; } = null!;

    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    [JsonPropertyName("host")]
    public string Host { get; set; } = null!;

    [JsonPropertyName("port")]
    public int Port { get; set; }

    [JsonPropertyName("macAddress")]
    [JsonConverter(typeof(PhysicalAddressJsonConverter))]
    public PhysicalAddress MacAddress { get; set; } = null!;
}

internal interface ITarget
{
    public string NormalizedName { get; }
    public string Name { get; }
    public string Host { get; }
    public int Port { get; }
    public PhysicalAddress MacAddress { get; }
}