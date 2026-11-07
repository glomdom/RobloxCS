using RobloxCS.Packages.Dependencies;
using Tomlyn;
using Tomlyn.Model;

namespace RobloxCS.Packages;

public sealed class ManifestException(string message) : Exception(message);

public sealed record ProjectManifest(
    string RobloxCSVersion,
    IReadOnlyDictionary<string, DependencySpec> Dependencies,
    IReadOnlyDictionary<string, string> Sources
) {
    public const string FileName = "rbxcs.toml";

    public static ProjectManifest Load(string root) {
        var path = Path.Combine(root, FileName);
        if (!File.Exists(path)) {
            throw new ManifestException($"{FileName} not found in {root}, run `rbxcsc init` first");
        }

        TomlTable table;

        try {
            table = TomlSerializer.Deserialize<TomlTable>(File.ReadAllText(path))!;
        } catch (TomlException ex) {
            throw new ManifestException($"{FileName}{ex.Message}");
        }

        if (GetString(table, "robloxcs") is not { } version) {
            throw new ManifestException($"{FileName} is missing `robloxcs = \"<version>\"`");
        }
        
        var dependencies = new Dictionary<string, DependencySpec>();
        if (table.TryGetValue("dependencies", out var rawDependencies) && rawDependencies is TomlTable dependencyTable) {
            foreach (var (name, value) in dependencyTable) {
                dependencies[name] = value switch {
                    string range => new DependencySpec(range, null, null, null),
                    TomlTable spec => new DependencySpec(GetString(spec, "version"), GetString(spec, "source"), GetString(spec, "git"), GetString(spec, "tag")),
                    _ => throw new ManifestException($"dependency '{name}' must be a version string or a table"),
                };
            }
        }

        var sources = new Dictionary<string, string>();
        if (!table.TryGetValue("sources", out var rawSources) || rawSources is not TomlTable sourceTable) {
            return new ProjectManifest(version, dependencies, sources);
        }
        
        foreach (var (name, value) in sourceTable) {
            sources[name] = value as string ?? throw new ManifestException($"source '{name}' must be a URL string");
        }

        return new ProjectManifest(version, dependencies, sources);
    }
    
    private static string? GetString(TomlTable table, string key) {
        return table.TryGetValue(key, out var value) ? value as string : null;
    }
}