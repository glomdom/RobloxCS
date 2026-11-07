using Tomlyn.Model;

namespace RobloxCS.Packages.Dependencies;

public static class DependencyTable {
    public static Dictionary<string, DependencySpec> Read(TomlTable table, string fileName) {
        var dependencies = new Dictionary<string, DependencySpec>(StringComparer.Ordinal);

        if (!table.TryGetValue("dependencies", out var raw) || raw is not TomlTable dependencyTable) {
            return dependencies;
        }

        foreach (var (name, value) in dependencyTable) {
            dependencies[name.ToLowerInvariant()] = value switch {
                string range => new DependencySpec(range, null, null, null),
                TomlTable spec => new DependencySpec(GetString(spec, "version"), GetString(spec, "source"), GetString(spec, "git"), GetString(spec, "tag")),

                _ => throw new PackageException($"{fileName}: dependency '{name}' must be a version string or a table"),
            };
        }

        return dependencies;
    }

    public static string? GetString(TomlTable table, string key) {
        return table.TryGetValue(key, out var value) ? value as string : null;
    }
}