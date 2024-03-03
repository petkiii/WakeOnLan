using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace WakeOnLan;

[SuppressMessage("AOT",
    "IL3050:Calling members annotated with \'RequiresDynamicCodeAttribute\' may break functionality when AOT compiling.")]
[SuppressMessage("Trimming",
    "IL2026:Members annotated with \'RequiresUnreferencedCodeAttribute\' require dynamic access otherwise can break functionality when trimming application code")]
internal static class DataContext
{
    private const string TargetsFile = "targets.json";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = DataContainerContext.Default,
        Converters = { new PhysicalAddressJsonConverter() }
    };

    private static readonly DataContainer DataContainer;
    public static IEnumerable<Target> Targets => DataContainer.Targets.Values;

    static DataContext()
    {
        if (!File.Exists(TargetsFile))
        {
            DataContainer = new DataContainer();
            File.WriteAllText(TargetsFile, "{\"Targets\":{}}");
            // Save();
        }
        else
        {
            var data = File.ReadAllText(TargetsFile);
            DataContainer = JsonSerializer.Deserialize<DataContainer>(data, SerializerOptions) ?? new DataContainer();
        }
    }

    public static void Add(Target target)
    {
        if (!DataContainer.Targets.TryAdd(target.NormalizedName, target))
            throw new InvalidOperationException("Target already exists.");
    }

    public static void Remove(Target target)
    {
        if (!DataContainer.Targets.Remove(target.NormalizedName))
            throw new InvalidOperationException("Target does not exist.");
    }

    public static void Save()
    {
        var targets = JsonSerializer.Serialize(DataContainer, SerializerOptions);
        File.WriteAllText(TargetsFile, targets);
    }
}