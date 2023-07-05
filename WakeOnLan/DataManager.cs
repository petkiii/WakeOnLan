using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace WakeOnLan;

[SuppressMessage("AOT",
    "IL3050:Calling members annotated with \'RequiresDynamicCodeAttribute\' may break functionality when AOT compiling.")]
[SuppressMessage("Trimming",
    "IL2026:Members annotated with \'RequiresUnreferencedCodeAttribute\' require dynamic access otherwise can break functionality when trimming application code")]
internal static class Data
{
    private const string TargetsFile = "targets.json";

    private static readonly JsonSerializerOptions SerializerOptions = new()
    {
        WriteIndented = true,
        TypeInfoResolver = DataJsonSerializerContext.Default,
        Converters = { new PhysicalAddressJsonConverter() }
    };

    private static DataContainer _dataContainer = null!;
    public static Dictionary<string, Target> Targets => _dataContainer.Targets!;

    public static void EnsureCreated()
    {
        if (!File.Exists(TargetsFile))
            File.WriteAllText(TargetsFile, "{\"Targets\":{}}");
    }

    public static void Add(Target target)
    {
        if (Targets.ContainsKey(target.NormalizedName))
            throw new InvalidOperationException("Target already exists.");

        Targets[target.NormalizedName] = target;
    }

    public static void Remove(Target target)
    {
        if (!Targets.ContainsKey(target.NormalizedName))
            throw new InvalidOperationException("Target does not exist.");

        Targets.Remove(target.NormalizedName);
    }

    public static void SaveChanges()
    {
        var targets = JsonSerializer.Serialize(_dataContainer, SerializerOptions);
        File.WriteAllText(TargetsFile, targets);
    }

    public static void Load()
    {
        var data = File.ReadAllText(TargetsFile);
        var targetsContainer = JsonSerializer.Deserialize<DataContainer>(data, SerializerOptions);

        targetsContainer ??= new DataContainer();
        targetsContainer.Targets ??= new Dictionary<string, Target>();

        _dataContainer = targetsContainer;
    }
}