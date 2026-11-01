using System.Reflection;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Serilog;

namespace RobloxCS.CLI;

internal static partial class TypesInstall {
    private const string ResourceName = "RobloxCS.Types.dll";

    public static string Version { get; } =
        typeof(TypesInstall).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+')[0] ?? "0.0.0";

    public static string Root =>
        Environment.GetEnvironmentVariable("RBXCS_HOME")
        ?? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".rbxcs");

    public static string AssemblyPath => Path.Combine(Root, Version, ResourceName);

    public static string EnsureInstalled() {
        using var resource = typeof(TypesInstall).Assembly.GetManifestResourceStream(ResourceName)
            ?? throw new InvalidOperationException($"'{ResourceName}' is not embedded in rbxcsc. This build is broken.");

        var bytes = new byte[resource.Length];
        resource.ReadExactly(bytes);

        var path = AssemblyPath;

        if (File.Exists(path) && SHA256.HashData(File.ReadAllBytes(path)).AsSpan().SequenceEqual(SHA256.HashData(bytes))) {
            return path;
        }

        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        var temp = $"{path}.{Environment.ProcessId}.tmp";

        try {
            File.WriteAllBytes(temp, bytes);
            File.Move(temp, path, true);

            Log.Information("Installed RobloxCS.Types {Version} to {Path}", Version, path);
        } catch (IOException ex) {
            File.Delete(temp);

            Log.Warning("Could not update {Path} ({Message}), it may be locked by an IDE", path, ex.Message);
        }

        return path;
    }

    public static string? FindProjectVersion(string root) {
        var candidates = Directory.EnumerateFiles(root, "Directory.Build.props")
            .Concat(Directory.EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories));

        foreach (var file in candidates) {
            if (ProjectVersionRegex().Match(File.ReadAllText(file)) is { Success: true } match) {
                return match.Groups["version"].Value.Trim();
            }
        }

        return null;
    }

    [GeneratedRegex(@"<RobloxCSVersion>(?<version>[^<]+)</RobloxCSVersion>")]
    private static partial Regex ProjectVersionRegex();
}