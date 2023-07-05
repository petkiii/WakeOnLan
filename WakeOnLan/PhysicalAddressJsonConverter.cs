using System.Net.NetworkInformation;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace WakeOnLan;

internal sealed class PhysicalAddressJsonConverter : JsonConverter<PhysicalAddress>
{
    public override PhysicalAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!PhysicalAddress.TryParse(reader.GetString()!, out var macAddress))
            throw new JsonException("Unable to convert mac address");

        return macAddress;

    }

    public override void Write(Utf8JsonWriter writer, PhysicalAddress value, JsonSerializerOptions options)
    {
		writer.WriteStringValue(value.ToString());
    }
}