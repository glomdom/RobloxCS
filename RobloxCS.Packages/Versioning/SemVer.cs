namespace RobloxCS.Packages.Versioning;

public readonly record struct SemVer(int Major, int Minor, int Patch, string? Prerelease) : IComparable<SemVer> {
    public SemVer Core => this with { Prerelease = null };

    public static bool TryParse(string text, out SemVer version) {
        version = default;

        var trimmed = text.Trim();
        if (trimmed.StartsWith('v') || trimmed.StartsWith('V')) {
            trimmed = trimmed[1..];
        }

        var plus = trimmed.IndexOf('+');
        if (plus >= 0) {
            trimmed = trimmed[..plus];
        }

        string? prerelease = null;
        var dash = trimmed.IndexOf('-');
        if (dash >= 0) {
            prerelease = trimmed[(dash + 1)..];
            trimmed = trimmed[..dash];
        }

        var parts = trimmed.Split('.');
        if (parts.Length != 3 || !int.TryParse(parts[0], out var major) || !int.TryParse(parts[1], out var minor) || !int.TryParse(parts[2], out var patch)) {
            return false;
        }

        version = new SemVer(major, minor, patch, prerelease is { Length: > 0 } ? prerelease : null);

        return true;
    }

    public int CompareTo(SemVer other) {
        var core = (Major, Minor, Patch).CompareTo((other.Major, other.Minor, other.Patch));
        if (core != 0) {
            return core;
        }

        return (Prerelease, other.Prerelease) switch {
            (null, null) => 0,
            (null, _) => 1,
            (_, null) => -1,
            _ => string.CompareOrdinal(Prerelease, other.Prerelease),
        };
    }

    public static bool operator <(SemVer left, SemVer right) => left.CompareTo(right) < 0;
    public static bool operator >(SemVer left, SemVer right) => left.CompareTo(right) > 0;
    public static bool operator <=(SemVer left, SemVer right) => left.CompareTo(right) <= 0;
    public static bool operator >=(SemVer left, SemVer right) => left.CompareTo(right) >= 0;

    public override string ToString() => Prerelease is null ? $"{Major}.{Minor}.{Patch}" : $"{Major}.{Minor}.{Patch}-{Prerelease}";
}