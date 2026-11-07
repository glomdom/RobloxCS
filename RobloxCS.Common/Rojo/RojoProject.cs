using System.Text.Json;
using RobloxCS.Common.Diagnostics;
using Serilog;

namespace RobloxCS.Common.Rojo;

public static class RojoProject {
    public static Result<List<RojoAnchor>> LoadAnchors(string projectFilePath) {
        var projectDir = Path.GetDirectoryName(Path.GetFullPath(projectFilePath));
        if (projectDir is null) {
            return Diagnostic.Error(DiagnosticId.FileNotFound, $"Could not determine directory of '{projectFilePath}'");
        }

        using var doc = JsonDocument.Parse(File.ReadAllText(projectFilePath));
        if (!doc.RootElement.TryGetProperty("tree", out var tree)) {
            return Diagnostic.Error(DiagnosticId.InvalidProjectJson, $"'{projectFilePath}' has no 'tree' element");
        }

        var anchors = new List<RojoAnchor>();
        Walk(tree, [], projectDir, anchors);

        return anchors;
    }

    public static bool TryResolveAnchor(string projectName, List<RojoAnchor> anchors, out RojoAnchor anchor) {
        anchor = null!;

        var suffix = projectName.Split('.').Last();
        var matches = anchors.Where(a => string.Equals(AnchorFolderName(a), suffix, StringComparison.OrdinalIgnoreCase)).ToList();

        switch (matches.Count) {
            case 1: {
                anchor = matches[0];

                return true;
            }

            case 0: {
                Log.Error(
                    "No $path in the Rojo project file maps to {ProjectName} (looking for a folder named '{Suffix}'). Available anchors: {Anchors}",
                    projectName, suffix, string.Join(", ", anchors.Select(AnchorFolderName))
                );

                return false;
            }

            default: {
                Log.Error(
                    "{ProjectName} matches {Count} anchors named '{Suffix}': {Anchors}. Disambiguate them in the project file.",
                    projectName, matches.Count, suffix, string.Join(", ", matches.Select(m => m.ToString()))
                );

                return false;
            }
        }
    }

    public static bool TryResolvePackageAnchor(string? projectFilePath, List<RojoAnchor> anchors, out RojoAnchor? anchor) {
        anchor = null;

        if (projectFilePath is null) {
            return false;
        }

        var separator = Path.DirectorySeparatorChar;
        var marker = $"{separator}.rbxcs{separator}packages{separator}";
        var full = Path.GetFullPath(projectFilePath);
        var index = full.IndexOf(marker, StringComparison.OrdinalIgnoreCase);

        if (index < 0) {
            return false;
        }

        var rest = full[(index + marker.Length)..];
        var packageDirectory = rest[..rest.IndexOf(separator)];
        var packages = anchors.FirstOrDefault(a => string.Equals(AnchorFolderName(a), "packages", StringComparison.OrdinalIgnoreCase));

        if (packages is not null) {
            anchor = new RojoAnchor(Path.Combine(packages.FullPath, packageDirectory), [.. packages.InstancePath, packageDirectory]);
        }

        return true;
    }

    private static string AnchorFolderName(RojoAnchor anchor) => Path.GetFileName(anchor.FullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

    private static void Walk(JsonElement node, List<string> instancePath, string projectDir, List<RojoAnchor> anchors) {
        if (node.ValueKind != JsonValueKind.Object) return;

        foreach (var prop in node.EnumerateObject()) {
            if (prop.Name == "$path") {
                var relative = prop.Value.ValueKind switch {
                    JsonValueKind.String => prop.Value.GetString(),
                    JsonValueKind.Object when prop.Value.TryGetProperty("optional", out var optional) => optional.GetString(),

                    _ => null,
                };

                if (relative is not null) {
                    anchors.Add(new RojoAnchor(Path.GetFullPath(Path.Combine(projectDir, relative)), [.. instancePath]));
                }

                continue;
            }

            if (prop.Name.StartsWith('$')) continue;

            instancePath.Add(prop.Name);
            Walk(prop.Value, instancePath, projectDir, anchors);
            instancePath.RemoveAt(instancePath.Count - 1);
        }
    }
}