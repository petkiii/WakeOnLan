using System.Text.Json.Serialization;

namespace WakeOnLan;

[JsonSerializable(typeof(DataContainer))]
internal partial class DataJsonSerializerContext : JsonSerializerContext
{

}
