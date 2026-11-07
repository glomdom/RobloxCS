namespace RobloxCS.Packages;

public static class PackageStore {
    private const string CommitMarker = ".rbxcs-commit";

    private const string DirectoryBuildProps = """
        <Project>
            <Import Project="rbxcs.props" Condition="Exists('rbxcs.props')" />
        </Project>

        """;

    public static string DirectoryFor(string root, string name) => Path.Combine(root, ".rbxcs", "packages", name.Replace('/', '.'));

    public static async Task<string> FetchAsync(string root, string name, string url, string tag, string commit, CancellationToken cancellation) {
        var directory = DirectoryFor(root, name);
        var marker = Path.Combine(directory, CommitMarker);

        if (File.Exists(marker) && (await File.ReadAllTextAsync(marker, cancellation)).Trim() == commit) {
            return directory;
        }

        var temp = $"{directory}.tmp";

        DeleteDirectory(temp);
        Directory.CreateDirectory(Path.GetDirectoryName(directory)!);

        await Git.RunAsync(null, cancellation, "clone", "--quiet", "--depth", "1", "--branch", tag, url, temp);

        var head = (await Git.RunAsync(temp, cancellation, "rev-parse", "HEAD")).Trim();
        if (head != commit) {
            DeleteDirectory(temp);

            throw new PackageException($"{name}@{tag} should be commit {commit[..12]} but the clone is at {head[..12]}, the tag was probably moved");
        }

        DeleteDirectory(directory);
        Directory.Move(temp, directory);

        if (!File.Exists(Path.Combine(directory, "Directory.Build.props"))) {
            await File.WriteAllTextAsync(Path.Combine(directory, "Directory.Build.props"), DirectoryBuildProps, cancellation);
        }

        await File.WriteAllTextAsync(marker, commit, cancellation);

        return directory;
    }

    public static bool IsInstalled(string root, LockedPackage package) {
        var marker = Path.Combine(DirectoryFor(root, package.Name), CommitMarker);

        return File.Exists(marker) && File.ReadAllText(marker).Trim() == package.Commit;
    }

    public static int Prune(string root, IEnumerable<LockedPackage> keep) {
        var packages = Path.Combine(root, ".rbxcs", "packages");
        if (!Directory.Exists(packages)) {
            return 0;
        }

        var wanted = keep.Select(package => DirectoryFor(root, package.Name)).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var removed = 0;

        foreach (var directory in Directory.EnumerateDirectories(packages)) {
            if (wanted.Contains(directory)) {
                continue;
            }

            DeleteDirectory(directory);
            removed++;
        }

        return removed;
    }

    public static void DeleteDirectory(string directory) {
        if (!Directory.Exists(directory)) {
            return;
        }

        foreach (var file in Directory.EnumerateFiles(directory, "*", SearchOption.AllDirectories)) {
            File.SetAttributes(file, FileAttributes.Normal);
        }

        Directory.Delete(directory, true);
    }
}