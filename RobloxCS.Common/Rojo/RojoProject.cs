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

    private static string AnchorFolderName(RojoAnchor anchor) => Path.GetFileName(anchor.FullPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar));

    private static void Walk(JsonElement node, List<string> instancePath, string projectDir, List<RojoAnchor> anchors) {
        if (node.ValueKind != JsonValueKind.Object) return;

        foreach (var prop in node.EnumerateObject()) {
            if (prop.Name == "$path") {
                var relative = prop.Value.ValueKind switch {
                    JsonValueKind.String => prop.Value.GetString(),

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