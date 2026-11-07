using RobloxCS.Packages.Dependencies;
using Tomlyn;
using Tomlyn.Model;

namespace RobloxCS.Packages;

public sealed record PackageManifest(
    string Name,
    string Kind,
    string? RobloxCS,
    string Project,
    IReadOnlyDictionary<string, DependencySpec> Dependencies
) {
    public const string FileName = "rbxcs.package.toml";

    public static PackageManifest Load(string directory) {
        var path = Path.Combine(directory, FileName);
        if (!File.Exists(path)) {
            throw new PackageException($"{directory} has no {FileName}, it isn't a RobloxCS package");
        }

        TomlTable table;

        try {
            table = TomlSerializer.Deserialize<TomlTable>(File.ReadAllText(path))!;
        } catch (TomlException ex) {
            throw new PackageException($"{path}({ex.Line},{ex.Column}): {ex.Message}");
        }

        var name = DependencyTable.GetString(table, "name") ?? throw new PackageException($"{path} is missing `name`");
        var project = DependencyTable.GetString(table, "project") ?? throw new PackageException($"{path} is missing `project`");

        return new PackageManifest(
            name.ToLowerInvariant(),
            DependencyTable.GetString(table, "kind") ?? "library",
            DependencyTable.GetString(table, "robloxcs"),
            project,
            DependencyTable.Read(table, path)
        );
    }
}