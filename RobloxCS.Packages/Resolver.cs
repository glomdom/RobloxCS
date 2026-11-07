using RobloxCS.Packages.Dependencies;
using RobloxCS.Packages.Versioning;
using Serilog;

namespace RobloxCS.Packages;

public sealed class Resolver {
    private readonly string _root;
    private readonly PackageSources _sources;
    private readonly string _compilerVersion;
    private readonly Lockfile _previous;
    private readonly Dictionary<string, Dictionary<string, string>> _tagCache = new(StringComparer.Ordinal);

    public Resolver(string root, string home, IReadOnlyDictionary<string, string> sources, string compilerVersion, Lockfile? previous = null) {
        _root = root;
        _sources = new PackageSources(home, sources);
        _compilerVersion = compilerVersion;
        _previous = previous ?? new Lockfile();
    }

    public async Task<List<LockedPackage>> ResolveAllAsync(IReadOnlyDictionary<string, DependencySpec> roots, CancellationToken cancellation) {
        var resolved = new Dictionary<string, LockedPackage>(StringComparer.Ordinal);
        var queue = new Queue<(string Name, DependencySpec Spec, string RequiredBy)>(
            roots.Select(pair => (pair.Key, pair.Value, ProjectManifest.FileName))
        );

        while (queue.TryDequeue(out var item)) {
            var package = await ResolveOneAsync(item.Name, item.Spec, cancellation);

            if (resolved.TryGetValue(package.Name, out var existing)) {
                if (existing.Commit != package.Commit) {
                    throw new PackageException(
                        $"conflicting versions of {package.Name}: {existing.Version} is already selected, but {item.RequiredBy} needs {item.Spec.Version ?? item.Spec.Tag}"
                    );
                }

                continue;
            }

            resolved[package.Name] = package;

            var manifest = PackageManifest.Load(PackageStore.DirectoryFor(_root, package.Name));

            foreach (var (name, spec) in manifest.Dependencies) {
                queue.Enqueue((name, spec, package.Name));
            }
        }

        return [.. resolved.Values.OrderBy(package => package.Name, StringComparer.Ordinal)];
    }

    public async Task<LockedPackage> ResolveOneAsync(string name, DependencySpec spec, CancellationToken cancellation) {
        if (FindLocked(name, spec) is { } locked) {
            Log.Verbose("Keeping locked {Package} {Version} ({Commit})", locked.Name, locked.Version, locked.Commit);

            return await InstallAsync(locked.Name, spec, locked.Git, locked.Tag, locked.Commit, locked.Version, cancellation);
        }

        var url = spec.Git ?? await _sources.FindRepositoryAsync(name, spec.Source, cancellation);

        if (!_tagCache.TryGetValue(url, out var tags)) {
            Log.Verbose("Listing tags of {Url}", url);

            tags = await Git.ListTagsAsync(url, cancellation);
            _tagCache[url] = tags;
        }

        var (tag, commit, version) = PickTag(name, spec, tags);

        Log.Verbose("Resolved {Package} to {Tag} ({Commit})", name, tag, commit);

        return await InstallAsync(name, spec, url, tag, commit, version, cancellation);
    }

    public async Task<LockedPackage> InstallAsync(string name, DependencySpec spec, string url, string tag, string commit, string version, CancellationToken cancellation) {
        var directory = await PackageStore.FetchAsync(_root, name, url, tag, commit, cancellation);
        var manifest = PackageManifest.Load(directory);

        if (spec.Git is null && manifest.Name != name) {
            throw new PackageException($"'{name}' points at a package named '{manifest.Name}'");
        }

        var canonical = PackageStore.DirectoryFor(_root, manifest.Name);

        if (!string.Equals(Path.GetFullPath(directory), Path.GetFullPath(canonical), StringComparison.OrdinalIgnoreCase)) {
            PackageStore.DeleteDirectory(canonical);
            Directory.Move(directory, canonical);

            directory = canonical;
        }

        CheckCompiler(manifest);

        var project = Path.Combine(directory, manifest.Project);
        if (!File.Exists(project)) {
            throw new PackageException($"{manifest.Name} declares project '{manifest.Project}' but that file doesn't exist");
        }

        return new LockedPackage(
            manifest.Name,
            version,
            url,
            tag,
            commit,
            Path.GetRelativePath(_root, project).Replace('\\', '/'),
            [.. manifest.Dependencies.Keys.Order(StringComparer.Ordinal)]
        );
    }

    private LockedPackage? FindLocked(string name, DependencySpec spec) {
        var locked = spec.Git is { } git
            ? _previous.Packages.FirstOrDefault(package => package.Git == git)
            : _previous.Packages.FirstOrDefault(package => package.Name == name);

        if (locked is null) {
            return null;
        }

        if (spec.Tag is { } tag) {
            return locked.Tag == tag ? locked : null;
        }

        if (spec.Git is not null) {
            return locked;
        }

        var range = spec.Version is { } text ? VersionRange.Parse(text) : VersionRange.Any;

        return SemVer.TryParse(locked.Version, out var version) && range.IsSatisfiedBy(version) ? locked : null;
    }

    private static (string Tag, string Commit, string Version) PickTag(string name, DependencySpec spec, Dictionary<string, string> tags) {
        if (spec.Tag is { } wanted) {
            if (!tags.TryGetValue(wanted, out var commit)) {
                throw new PackageException($"{name} has no tag '{wanted}'");
            }

            return (wanted, commit, SemVer.TryParse(wanted, out var tagged) ? tagged.ToString() : wanted);
        }

        var range = spec.Version is { } text ? VersionRange.Parse(text) : VersionRange.Any;

        (string Tag, string Commit, SemVer Version)? best = null;

        foreach (var (tag, commit) in tags) {
            if (!SemVer.TryParse(tag, out var version) || !range.IsSatisfiedBy(version)) {
                continue;
            }

            if (best is null || version > best.Value.Version) {
                best = (tag, commit, version);
            }
        }

        if (best is not { } chosen) {
            throw new PackageException($"no version of {name} matches '{spec.Version ?? "*"}'");
        }

        return (chosen.Tag, chosen.Commit, chosen.Version.ToString());
    }

    private void CheckCompiler(PackageManifest manifest) {
        if (manifest.RobloxCS is not { } requirement || !SemVer.TryParse(_compilerVersion, out var compiler)) {
            return;
        }

        if (!VersionRange.Parse(requirement).IsSatisfiedBy(compiler.Core)) {
            throw new PackageException($"{manifest.Name} needs RobloxCS {requirement}, this project uses {_compilerVersion}");
        }
    }
}