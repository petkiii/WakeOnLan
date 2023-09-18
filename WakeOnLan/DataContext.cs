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
        TypeInfoResolver = DataJsonSerializerContext.Default,
        Converters = { new PhysicalAddressJsonConverter() }
    };

    private static DataContainer _dataContainer = null!;
    public static IEnumerable<Target> Targets => _dataContainer.Targets!.Values;

    public static void Initialize()
    {
        if (!File.Exists(TargetsFile))
        {
            _dataContainer = new DataContainer
            {
                Targets = new Dictionary<string, Target>()
            };
            File.WriteAllText(TargetsFile, "{\"Targets\":{}}");
            // Save();
        }
        else
        {
            var data = File.ReadAllText(TargetsFile);
            _dataContainer = JsonSerializer.Deserialize<DataContainer>(data, SerializerOptions) ?? new DataContainer();
            _dataContainer.Targets ??= new Dictionary<string, Target>();
        }
    }

    public static void Add(Target target)
    {
        if (!_dataContainer.Targets!.TryAdd(target.NormalizedName, target))
            throw new InvalidOperationException("Target already exists.");
    }

    public static void Remove(Target target)
    {
        if (!_dataContainer.Targets!.Remove(target.NormalizedName))
            throw new InvalidOperationException("Target does not exist.");
    }

    public static void Save()
    {
        var targets = JsonSerializer.Serialize(_dataContainer, SerializerOptions);
        File.WriteAllText(TargetsFile, targets);
    }
}