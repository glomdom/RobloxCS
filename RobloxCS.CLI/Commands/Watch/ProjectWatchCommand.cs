using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Channels;
using JetBrains.Annotations;
using RobloxCS.CLI.Commands.Compile;
using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline;
using RobloxCS.CompilerPipeline.Solutions;
using Spectre.Console;
using Spectre.Console.Cli;

namespace RobloxCS.CLI.Commands.Watch;

[Description("Compile a C# project, then recompile files as they change.")]
[UsedImplicitly]
public sealed class ProjectWatchCommand : AsyncCommand<ProjectWatchCommand.Settings> {
    private static readonly TimeSpan Debounce = TimeSpan.FromMilliseconds(150);
    private const string ReloadMarker = "\0reload";

    private readonly IAnsiConsole _console;

    public ProjectWatchCommand(IAnsiConsole console) {
        _console = console;
    }

    public sealed class Settings : CompileSettings {
        [CommandOption("--project")]
        [Description("File path to Rojo's project JSON")]
        public string ProjectJsonPath { get; init; } = "default.project.json";
    }

    protected override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellation) {
        LoggerSetup.ApplyVerbosity(settings.Verbosity);
        
        var cwd = Environment.CurrentDirectory;
        var discovery = SolutionDiscovery.Discover(cwd);
        if (!discovery.Ok) {
            DiagnosticMarkup.Print(_console, discovery.Diagnostic);

            return -1;
        }

        var projectFile = Path.Combine(cwd, settings.ProjectJsonPath);
        if (!File.Exists(projectFile)) {
            _console.MarkupLine($"[red]error:[/] Failed to find {Markup.Escape(settings.ProjectJsonPath)} in {Markup.Escape(cwd)}.");

            return -1;
        }

        MsBuildRegister.RegisterDefaults();

        var options = new EmitOptions(settings.PrintHir, settings.SkipDiagnostics);

        return await RunAsync(discovery.Value, Path.GetFullPath(projectFile), options, cancellation);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private async Task<int> RunAsync(string slnFile, string projectFile, EmitOptions options, CancellationToken cancellation) {
        var root = Path.GetDirectoryName(Path.GetFullPath(slnFile))!;
        var changes = Channel.CreateUnbounded<string>();

        using var watcher = CreateWatcher(root, changes.Writer);

        var session = await OpenAndCompileAsync(slnFile, projectFile, options, root, cancellation);

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
                    if (session is null || batch.Any(path => RequiresReload(path, projectFile))) {
                        session?.Dispose();
                        session = null;

                        _console.MarkupLine("[grey]Project files changed, reloading...[/]");

                        session = await OpenAndCompileAsync(slnFile, projectFile, options, root, cancellation);

                        continue;
                    }

                    var sources = batch.Where(path => IsSource(path, root)).ToList();
                    if (sources.Count == 0) {
                        continue;
                    }

                    var update = await session.ApplyChangeAsync(sources, cancellation);

                    Report(update, root, stopwatch.Elapsed);
                } catch (OperationCanceledException) when (cancellation.IsCancellationRequested) {
                    break;
                } catch (Exception ex) {
                    _console.WriteLine(ex.ToString());
                    _console.MarkupLine("[cyan]note:[/] This may be a compiler bug, please report it at https://github.com/glomdom/RobloxCS");
                    _console.MarkupLine("[grey]Still watching.[/]");
                }
            }
        } finally {
            session?.Dispose();
        }

        return 0;
    }

    private async Task<ProjectSession?> OpenAndCompileAsync(string slnFile, string projectFile, EmitOptions options, string root, CancellationToken cancellation) {
        var stopwatch = Stopwatch.StartNew();
        var openDiagnostics = new List<Diagnostic>();
        var opened = await ProjectSession.OpenAsync(slnFile, projectFile, options, openDiagnostics, cancellation);

        DiagnosticMarkup.PrintAll(_console, openDiagnostics);

        if (!opened.Ok) {
            DiagnosticMarkup.Print(_console, opened.Diagnostic);
            _console.MarkupLine($"{Now()}[grey]waiting for changes to retry[/]");

            return null;
        }

        var session = opened.Value;
        var compiled = 0;
        var failed = 0;

        await foreach (var result in session.CompileAllAsync(cancellation)) {
            if (result.Output.Ok) {
                compiled++;

                continue;
            }

            failed++;

            _console.MarkupLine($"{Now()}[red]✗[/] {Markup.Escape(result.Document.Name)}");
            DiagnosticMarkup.PrintAll(_console, result.Diagnostics);
            DiagnosticMarkup.Print(_console, result.Output.Diagnostic);
        }

        var files = compiled == 1 ? "1 file" : $"{compiled} files";
        var failures = failed == 0 ? string.Empty : $", [red]{failed} failed[/]";

        _console.MarkupLine($"{Now()}compiled {files}{failures} in {FormatElapsed(stopwatch.Elapsed)}, [grey]watching for changes[/]");

        return session;
    }

    private static string FormatElapsed(TimeSpan elapsed) {
        return elapsed.TotalSeconds >= 1 ? $"{elapsed.TotalSeconds:0.0} s" : $"{(int)elapsed.TotalMilliseconds} ms";
    }

    private void Report(SessionUpdate update, string root, TimeSpan elapsed) {
        if (update.Results.Count == 0 && update.DeletedOutputs.Count == 0 && update.Failure is null && update.Diagnostics.Count == 0) {
            return;
        }

        DiagnosticMarkup.PrintAll(_console, update.Diagnostics);

        if (update.Failure is { } failure) {
            DiagnosticMarkup.Print(_console, failure);

            return;
        }

        foreach (var deleted in update.DeletedOutputs) {
            _console.MarkupLine($"{Now()}[yellow]-[/] [grey]{Markup.Escape(Path.GetRelativePath(root, deleted))}[/]");
        }

        var ms = $"[grey]{(int)elapsed.TotalMilliseconds} ms[/]";
        var failed = update.Results.Where(result => !result.Output.Ok).ToList();
        var succeeded = update.Results.Where(result => result.Output.Ok).ToList();

        if (succeeded.Count == 1 && failed.Count == 0) {
            var only = succeeded[0];
            var output = Markup.Escape(Path.GetRelativePath(root, only.Output.Value!.OutPath));

            DiagnosticMarkup.PrintAll(_console, only.Diagnostics);
            _console.MarkupLine($"{Now()}[green]✓[/] {Markup.Escape(only.Document.Name)}     [grey]{output}[/]  {ms}");

            return;
        }

        if (succeeded.Count > 0) {
            var names = string.Join(", ", succeeded.Select(result => Markup.Escape(result.Document.Name)));

            _console.MarkupLine($"{Now()}[green]✓[/] {names}  {ms}");
        }

        foreach (var result in failed) {
            _console.MarkupLine($"{Now()}[red]✗[/] {Markup.Escape(result.Document.Name)}");
            DiagnosticMarkup.PrintAll(_console, result.Diagnostics);
            DiagnosticMarkup.Print(_console, result.Output.Diagnostic!);
        }
    }

    private void ReportResult(DocumentResult result, string root) {
        DiagnosticMarkup.PrintAll(_console, result.Diagnostics);

        if (!result.Output.Ok) {
            DiagnosticMarkup.Print(_console, result.Output.Diagnostic);

            return;
        }

        var output = Path.GetRelativePath(root, result.Output.Value.OutPath);

        _console.MarkupLine($"[green]OK[/] {Markup.Escape(result.Document.Name)} [grey]-> {Markup.Escape(output)}[/]");
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

    private static bool RequiresReload(string path, string projectFile) {
        if (path == ReloadMarker) {
            return true;
        }

        if (string.Equals(Path.GetFullPath(path), projectFile, StringComparison.OrdinalIgnoreCase)) {
            return true;
        }

        return Path.GetExtension(path).ToLowerInvariant() is ".csproj" or ".slnx" or ".props" or ".targets";
    }

    private static bool IsSource(string path, string root) {
        if (!path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase)) {
            return false;
        }

        var segments = Path.GetRelativePath(root, path).Split(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

        return !segments.Any(segment => segment.Equals("bin", StringComparison.OrdinalIgnoreCase) || segment.Equals("obj", StringComparison.OrdinalIgnoreCase));
    }
    
    private static string Now() => $"[grey]{DateTime.Now:HH:mm:ss}[/]  ";
}