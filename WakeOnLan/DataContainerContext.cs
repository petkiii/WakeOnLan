using System.Text.Json.Serialization;

namespace WakeOnLan;

[JsonSerializable(typeof(DataContainer))]
internal sealed partial class DataContainerContext : JsonSerializerContext;