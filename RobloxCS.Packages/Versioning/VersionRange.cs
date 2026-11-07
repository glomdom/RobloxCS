namespace RobloxCS.Packages.Versioning;

public sealed class VersionRange {
    private readonly List<(SemVer? Lower, bool LowerInclusive, SemVer? Upper, bool UpperInclusive)> _bounds;
    private readonly bool _mentionsPrerelease;

    private VersionRange(List<(SemVer?, bool, SemVer?, bool)> bounds, bool mentionsPrerelease) {
        _bounds = bounds;
        _mentionsPrerelease = mentionsPrerelease;
    }

    public static VersionRange Any { get; } = new([], false);

    public static VersionRange Parse(string text) {
        var trimmed = text.Trim();
        if (trimmed is "" or "*") {
            return Any;
        }

        var bounds = new List<(SemVer?, bool, SemVer?, bool)>();

        foreach (var token in trimmed.Split([',', ' '], StringSplitOptions.RemoveEmptyEntries)) {
            bounds.Add(ParseComparator(token));
        }

        return new VersionRange(bounds, trimmed.Contains('-'));
    }

    public bool IsSatisfiedBy(SemVer version) {
        if (version.Prerelease is not null && !_mentionsPrerelease) {
            return false;
        }

        foreach (var (lower, lowerInclusive, upper, upperInclusive) in _bounds) {
            if (lower is { } min && (lowerInclusive ? version < min : version <= min)) {
                return false;
            }

            if (upper is { } max && (upperInclusive ? version > max : version >= max)) {
                return false;
            }
        }

        return true;
    }

    private static (SemVer?, bool, SemVer?, bool) ParseComparator(string token) {
        var op = token switch {
            _ when token.StartsWith(">=") => ">=",
            _ when token.StartsWith("<=") => "<=",
            _ when token.StartsWith('>') => ">",
            _ when token.StartsWith('<') => "<",
            _ when token.StartsWith('=') => "=",
            _ when token.StartsWith('^') => "^",
            _ when token.StartsWith('~') => "~",
            _ => "",
        };

        var (version, parts) = ParsePartial(token[op.Length..], token);

        return op switch {
            ">=" => (version, true, null, false),
            ">" => (version, false, null, false),
            "<=" => (null, false, version, true),
            "<" => (null, false, version, false),
            "=" => (version, true, version, true),
            "~" => (version, true, parts >= 2 ? new SemVer(version.Major, version.Minor + 1, 0, null) : new SemVer(version.Major + 1, 0, 0, null), false),
            _ => (version, true, CaretUpper(version, parts), false),
        };
    }

    private static SemVer CaretUpper(SemVer version, int parts) {
        if (version.Major > 0 || parts == 1) {
            return new SemVer(version.Major + 1, 0, 0, null);
        }

        if (version.Minor > 0 || parts == 2) {
            return new SemVer(0, version.Minor + 1, 0, null);
        }

        return new SemVer(0, 0, version.Patch + 1, null);
    }

    private static (SemVer Version, int Parts) ParsePartial(string text, string token) {
        var core = text;
        string? prerelease = null;

        var dash = core.IndexOf('-');
        if (dash >= 0) {
            prerelease = core[(dash + 1)..];
            core = core[..dash];
        }

        var parts = core.Split('.');
        var numbers = new int[3];

        if (parts.Length is < 1 or > 3) {
            throw new PackageException($"'{token}' is not a valid version requirement");
        }

        for (var index = 0; index < parts.Length; index++) {
            if (!int.TryParse(parts[index], out numbers[index])) {
                throw new PackageException($"'{token}' is not a valid version requirement");
            }
        }

        return (new SemVer(numbers[0], numbers[1], numbers[2], prerelease), parts.Length);
    }
}