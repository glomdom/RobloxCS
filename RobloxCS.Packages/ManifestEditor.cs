using System.Text.RegularExpressions;
using RobloxCS.Packages.Dependencies;

namespace RobloxCS.Packages;

public static class ManifestEditor {
    private const string Section = "[dependencies]";

    public static void SetDependency(string root, string name, DependencySpec spec) {
        var path = Path.Combine(root, ProjectManifest.FileName);
        var lines = File.ReadAllLines(path).ToList();
        var entry = $"\"{name}\" = {Format(spec)}";

        var (start, end) = FindSection(lines);

        if (start < 0) {
            if (lines.Count > 0 && lines[^1].Trim().Length > 0) {
                lines.Add(string.Empty);
            }

            lines.Add(Section);
            lines.Add(entry);
        } else if (FindEntry(lines, start, end, name) is >= 0 and var existing) {
            lines[existing] = entry;
        } else {
            var insertAt = end;
            while (insertAt > start + 1 && lines[insertAt - 1].Trim().Length == 0) {
                insertAt--;
            }

            lines.Insert(insertAt, entry);
        }

        File.WriteAllLines(path, lines);
    }

    public static bool RemoveDependency(string root, string name) {
        var path = Path.Combine(root, ProjectManifest.FileName);
        var lines = File.ReadAllLines(path).ToList();
        var (start, end) = FindSection(lines);
        var existing = start < 0 ? -1 : FindEntry(lines, start, end, name);

        if (existing < 0) {
            return false;
        }

        lines.RemoveAt(existing);
        File.WriteAllLines(path, lines);

        return true;
    }

    private static string Format(DependencySpec spec) {
        if (spec.Git is { } git) {
            return spec.Tag is { } tag ? $"{{ git = \"{git}\", tag = \"{tag}\" }}" : $"{{ git = \"{git}\" }}";
        }

        if (spec.Source is { } source) {
            return $"{{ source = \"{source}\", version = \"{spec.Version ?? "*"}\" }}";
        }

        return $"\"{spec.Version ?? "*"}\"";
    }

    private static (int Start, int End) FindSection(List<string> lines) {
        var start = lines.FindIndex(line => line.Trim() == Section);
        if (start < 0) {
            return (-1, -1);
        }

        var end = start + 1;
        while (end < lines.Count && !lines[end].TrimStart().StartsWith('[')) {
            end++;
        }

        return (start, end);
    }

    private static int FindEntry(List<string> lines, int start, int end, string name) {
        var key = new Regex($"^\\s*\"?{Regex.Escape(name)}\"?\\s*=", RegexOptions.IgnoreCase);

        for (var index = start + 1; index < end; index++) {
            if (key.IsMatch(lines[index])) {
                return index;
            }
        }

        return -1;
    }
}