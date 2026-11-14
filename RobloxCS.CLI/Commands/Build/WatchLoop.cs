using System.Diagnostics;
using System.Threading.Channels;
using RobloxCS.CLI.Reporting;
using RobloxCS.CompilerPipeline;

namespace RobloxCS.CLI.Commands.Build;

internal sealed class WatchLoop {
    private static readonly TimeSpan Debounce = TimeSpan.FromMilliseconds(150);
    private const string ReloadMarker = "\0reload";

    private readonly BuildReporter _reporter;
    private readonly ProjectLocation _location;
    private readonly EmitOptions _options;

    public WatchLoop(BuildReporter reporter, ProjectLocation location, EmitOptions options) {
        _reporter = reporter;
        _location = location;
        _options = options;
    }

    public async Task<int> RunAsync(CancellationToken cancellation) {
        var changes = Channel.CreateUnbounded<string>();

        using var watcher = CreateWatcher(_location.Root, changes.Writer);

        var session = await OpenAsync(cancellation);

        try {
            while (!cancellation.IsCancellationRequested) {
                HashSet<string> batch;

                try {
                    batch = await NextBatchAsync(changes.Reader, cancellation);
                } catch (OperationCanceledException) {
                    break;
                }

                var stopwatch = Stopwatch.StartNew();

                try {
                    if (session is null || batch.Any(RequiresReload)) {
                        session?.Dispose();
                        session = null;

                        _reporter.Reloading();

                        session = await OpenAsync(cancellation);

                        continue;
                    }

                    var sources = batch.Where(IsSource).ToList();
                    if (sources.Count == 0) {
                        continue;
                    }

                    var update = await _reporter.RunAsync("build", "Rebuilding changed sources", _ => session.ApplyChangeAsync(sources, cancellation), announce: false);

                    _reporter.Update(update, stopwatch.Elapsed);
                } catch (OperationCanceledException) when (cancellation.IsCancellationRequested) {
                    break;
                } catch (Exception ex) {
                    _reporter.InternalError(ex);
                }
            }
        } finally {
            session?.Dispose();
            _reporter.Stopped();
        }

        return 0;
    }

    private async Task<ProjectSession?> OpenAsync(CancellationToken cancellation) {
        var outcome = await ProjectBuild.OpenAndCompileAsync(_reporter, _location, _options, cancellation);

        _reporter.Watching(outcome.Session is null);

        return outcome.Session;
    }

    private static FileSystemWatcher CreateWatcher(string root, ChannelWriter<string> changes) {
        var watcher = new FileSystemWatcher(root) {
            IncludeSubdirectories = true,
            InternalBufferSize = 64 * 1024,
            NotifyFilter = NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.Size,
        };

        watcher.Changed += (_, e) => changes.TryWrite(e.FullPath);
        watcher.Created += (_, e) => changes.TryWrite(e.FullPath);
        watcher.Deleted += (_, e) => changes.TryWrite(e.FullPath);
        watcher.Renamed += (_, e) => {
            changes.TryWrite(e.OldFullPath);
            changes.TryWrite(e.FullPath);
        };

        watcher.Error += (_, _) => changes.TryWrite(ReloadMarker);

        watcher.EnableRaisingEvents = true;

        return watcher;
    }

    private static async Task<HashSet<string>> NextBatchAsync(ChannelReader<string> reader, CancellationToken cancellation) {
        var batch = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { await reader.ReadAsync(cancellation) };

        while (true) {
            using var quiet = CancellationTokenSource.CreateLinkedTokenSource(cancellation);
            quiet.CancelAfter(Debounce);

            try {
                batch.Add(await reader.ReadAsync(quiet.Token));
            } catch (OperationCanceledException) when (!cancellation.IsCancellationRequested) {
                return batch;
            }
        }
    }

    private bool RequiresReload(string path) {
        if (path == ReloadMarker) {
            return true;
        }

        if (string.Equals(Path.GetFullPath(path), _location.ProjectFile, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }

        return Path.GetExtension(path).ToLowerInvariant() is ".csproj" or ".slnx" or ".props" or ".targets";
    }

    private bool IsSource(string path) {
        if (!path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) {
            return false;
        }

        var segments = Path.GetRelativePath(_location.Root, path).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        return !segments.Any(segment => segment.Equals("bin", StringComparison.OrdinalIgnoreCase) || segment.Equals("obj", StringComparison.OrdinalIgnoreCase));
    }
}