using System.Text.Json;

namespace RobloxCS.Packages;

public sealed record LockedPackage(
    string Name,
    string Version,
    string Git,
    string Tag,
    string Commit,
    string Project,
    List<string> Dependencies
);

public sealed class Lockfile {
    public const string FileName = "rbxcs.lock";

    private static readonly JsonSerializerOptions Options = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        PropertyNameCaseInsensitive = true,
    };

    public int Version { get; set; } = 1;
    public List<LockedPackage> Packages { get; set; } = [];

    public static Lockfile Load(string root) {
        var path = Path.Combine(root, FileName);
        if (!File.Exists(path)) {
            return new Lockfile();
        }

        try {
            return JsonSerializer.Deserialize<Lockfile>(File.ReadAllText(path), Options) ?? new Lockfile();
        } catch (JsonException ex) {
            throw new PackageException($"{FileName} is corrupt ({ex.Message}), delete it and run `rbxcsc package update`");
        }
    }

    public void Save(string root) {
        File.WriteAllText(Path.Combine(root, FileName), JsonSerializer.Serialize(this, Options) + "\n");
    }
}