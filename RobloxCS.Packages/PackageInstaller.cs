namespace RobloxCS.Packages;

public static class PackageInstaller {
    public static int Apply(string root, string robloxCSVersion, List<LockedPackage> packages) {
        new Lockfile { Packages = packages }.Save(root);

        var projects = packages.ToDictionary(package => package.Name, package => Path.GetFullPath(Path.Combine(root, package.Project)));

        Props.Write(root, robloxCSVersion, projects.Values);

        foreach (var package in packages) {
            var dependencyProjects = package.Dependencies.Select(name => projects[name]);

            Props.Write(PackageStore.DirectoryFor(root, package.Name), robloxCSVersion, dependencyProjects);
        }

        InvalidateRestore(root);

        return PackageStore.Prune(root, packages);
    }

    public static bool NeedsRestore(string root) {
        return Lockfile.Load(root).Packages.Any(package => !PackageStore.IsInstalled(root, package));
    }

    public static async Task<List<LockedPackage>> RestoreAsync(string root, string home, ProjectManifest manifest, CancellationToken cancellation) {
        var resolver = new Resolver(root, home, manifest.Sources, manifest.RobloxCSVersion, Lockfile.Load(root));
        var packages = await resolver.ResolveAllAsync(manifest.Dependencies, cancellation);

        Apply(root, manifest.RobloxCSVersion, packages);

        return packages;
    }

    public static void InvalidateRestore(string root) {
        foreach (var project in Directory.EnumerateFiles(root, "*.csproj", SearchOption.AllDirectories)) {
            var assets = Path.Combine(Path.GetDirectoryName(project)!, "obj", "project.assets.json");

            if (File.Exists(assets)) {
                File.Delete(assets);
            }
        }
    }
}