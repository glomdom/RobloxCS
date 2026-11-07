using System.Security.Cryptography;
using System.Text;
using RobloxCS.Packages.Dependencies;
using Tomlyn;
using Tomlyn.Model;

namespace RobloxCS.Packages;

public sealed class PackageSources {
    public const string DefaultName = "rbxcs";
    public const string DefaultUrl = "https://github.com/glomdom/rbxcs-packages";

    private static readonly TimeSpan RefreshAfter = TimeSpan.FromMinutes(10);

    private readonly string _home;
    private readonly IReadOnlyDictionary<string, string> _configured;
    private readonly HashSet<string> _synced = [];

    public PackageSources(string home, IReadOnlyDictionary<string, string> configured) {
        _home = home;
        _configured = configured;
    }

    public async Task<string> FindRepositoryAsync(string name, string? source, CancellationToken cancellation) {
        var sourceName = source ?? DefaultName;
        var url = _configured.GetValueOrDefault(sourceName)
            ?? (sourceName == DefaultName ? DefaultUrl : throw new PackageException($"unknown source '{sourceName}', add it under [sources] in rbxcs.toml"));

        var directory = Path.Combine(_home, "sources", $"{sourceName}-{Hash(url)}");

        if (_synced.Add(directory)) {
            await SyncAsync(url, directory, cancellation);
        }

        var slash = name.IndexOf('/');
        var entry = Path.Combine(directory, "packages", name[..slash], $"{name[(slash + 1)..]}.toml");

        if (!File.Exists(entry)) {
            throw new PackageException($"'{name}' was not found in source '{sourceName}'");
        }

        try {
            var table = TomlSerializer.Deserialize<TomlTable>(await File.ReadAllTextAsync(entry, cancellation))!;

            return DependencyTable.GetString(table, "repository") ?? throw new PackageException($"{entry} is missing `repository`");
        } catch (TomlException ex) {
            throw new PackageException($"{entry}({ex.Line},{ex.Column}): {ex.Message}");
        }
    }

    private static async Task SyncAsync(string url, string directory, CancellationToken cancellation) {
        var marker = Path.Combine(directory, ".rbxcs-synced");

        if (!Directory.Exists(Path.Combine(directory, ".git"))) {
            PackageStore.DeleteDirectory(directory);
            await Git.RunAsync(null, cancellation, "clone", "--quiet", "--depth", "1", url, directory);
        } else if (!File.Exists(marker) || DateTime.UtcNow - File.GetLastWriteTimeUtc(marker) > RefreshAfter) {
            await Git.RunAsync(directory, cancellation, "fetch", "--quiet", "--depth", "1", "origin");
            await Git.RunAsync(directory, cancellation, "reset", "--quiet", "--hard", "origin/HEAD");
        } else {
            return;
        }

        await File.WriteAllTextAsync(marker, url, cancellation);
    }

    private static string Hash(string text) => Convert.ToHexStringLower(SHA256.HashData(Encoding.UTF8.GetBytes(text)))[..8];
}