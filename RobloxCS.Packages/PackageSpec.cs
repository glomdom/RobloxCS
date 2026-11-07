using System.Text.RegularExpressions;
using RobloxCS.Packages.Dependencies;

namespace RobloxCS.Packages;

public sealed class PackageSpecException(string message) : Exception(message);

public static partial class PackageSpec {
    public static (string Name, DependencySpec Spec) Parse(string spec) {
        spec = spec.Trim();
        if (spec.StartsWith("github:", StringComparison.Ordinal)) {
            var (repo, shortTag) = SplitTag(spec["github:".Length..]);

            return FromGit($"https://github.com/{repo}", shortTag);
        }

        if (spec.Contains("://", StringComparison.Ordinal) || spec.StartsWith("git@", StringComparison.Ordinal)) {
            var (url, urlTag) = SplitTag(spec);

            return FromGit(url, urlTag);
        }

        var (name, version) = SplitTag(spec);

        if (!NameRegex().IsMatch(name)) {
            throw new PackageSpecException($"'{name}' is not a valid package name, expected 'owner/name'");
        }

        if (version is { Length: 0 }) {
            throw new PackageSpecException($"'{spec}' has an empty version after '@'");
        }

        return (name.ToLowerInvariant(), new DependencySpec(version, null, null, null));
    }

    private static (string Name, DependencySpec Spec) FromGit(string url, string? tag) {
        var trimmed = url.TrimEnd('/');
        if (trimmed.EndsWith(".git", StringComparison.Ordinal)) {
            trimmed = trimmed[..^".git".Length];
        }

        var parts = trimmed.Split('/', ':');
        if (parts.Length < 2) {
            throw new PackageSpecException($"can't work out a package name from '{url}'");
        }

        var name = $"{parts[^2]}/{parts[^1]}".ToLowerInvariant();

        return (name, new DependencySpec(null, null, url, tag is { Length: > 0 } ? tag : null));
    }

    private static (string Value, string? Tag) SplitTag(string spec) {
        var at = spec.LastIndexOf('@');
        var slash = spec.LastIndexOfAny(['/', ':']);

        return at > slash ? (spec[..at], spec[(at + 1)..]) : (spec, null);
    }

    [GeneratedRegex("^[A-Za-z0-9][A-Za-z0-9_.-]*/[A-Za-z0-9][A-Za-z0-9_.-]*$")]
    private static partial Regex NameRegex();
}