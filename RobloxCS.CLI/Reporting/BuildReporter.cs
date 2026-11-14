using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline;
using Serilog;
using Spectre.Console;

namespace RobloxCS.CLI.Reporting;

internal sealed class BuildReporter {
    private readonly IAnsiConsole _console;
    private readonly TerminalOutput _output;
    private readonly string _root;
    private readonly HashSet<Diagnostic> _seen = [];
    private int _warnings;
    private int _errors;

    public BuildReporter(IAnsiConsole console, string root) {
        _console = console;
        _output = new TerminalOutput(console);
        _root = root;
    }

    public void Begin(string name) {
        Reset();
        _output.Write("build", "cyan", $"[bold]{Escape(name)}[/]");
    }

    public void Status(string markup) => _output.Write("build", "cyan", markup);

    public async Task<T> RunAsync<T>(string label, string markup, Func<Action<string>, Task<T>> action, bool announce = true) {
        using var _ = Serilog.Context.LogContext.PushProperty("ProjectRoot", _root);

        return await _output.RunAsync(label, markup, action, announce);
    }

    public void PrintDiagnostics(IEnumerable<Diagnostic> diagnostics) {
        foreach (var diagnostic in diagnostics) {
            PrintDiagnostic(diagnostic);
        }
    }

    public void PrintDiagnostic(Diagnostic diagnostic) {
        if (!_seen.Add(diagnostic)) {
            return;
        }

        if (diagnostic.Severity == Severity.Warning) {
            _warnings++;
        } else if (diagnostic.IsError) {
            _errors++;
        }

        DiagnosticMarkup.Print(_console, diagnostic, _root);
    }

    public void Failure(DocumentResult result) {
        _output.Write("failed", "red", Escape(Source(result)));
        PrintDiagnostics(result.Diagnostics);

        if (result.Output.Diagnostic is { } diagnostic &&
            (!result.Diagnostics.Any(item => item.IsError) || diagnostic.Id is not (DiagnosticId.SourceDidNotCompile or DiagnosticId.TranspilationFailed))) {
            PrintDiagnostic(diagnostic);
        }
    }

    public void Summary(int compiled, int failed, TimeSpan elapsed, int removed = 0) {
        var success = failed == 0 && _errors == 0;
        var files = compiled == 1 ? "1 file" : $"{compiled} files";
        var failures = failed == 0 ? string.Empty : $" · [red]{failed} failed[/]";
        var diagnostics = string.Empty;
        if (_errors > 0) {
            diagnostics += $" · [red]{_errors} {Plural(_errors, "error")}[/]";
        }

        if (_warnings > 0) {
            diagnostics += $" · [yellow]{_warnings} {Plural(_warnings, "warning")}[/]";
        }

        var title = success ? "[green bold]Build succeeded[/]" : "[red bold]Build failed[/]";
        if (success && removed > 0 && compiled == 0) {
            title = "[green bold]Outputs updated[/]";
        }

        var built = compiled > 0 || success && removed == 0 ? $"{files} built · " : string.Empty;
        var deletions = removed == 0 ? string.Empty : $" · {removed} {Plural(removed, "output")} removed";

        _output.Write(success ? "success" : "failed", success ? "green" : "red", $"{title} [grey]· {built}{Elapsed(elapsed)}[/]{failures}{deletions}{diagnostics}");
    }

    public void Watching(bool retry = false) {
        _output.Write("watch", "cyan", retry
            ? "Waiting for changes to retry [grey]· Ctrl+C to stop[/]"
            : "Watching for changes [grey]· Ctrl+C to stop[/]");
    }

    public void Reloading() => _output.Write("reload", "cyan", "Project configuration changed. Reloading…");

    public void Stopped() => _output.Write("stopped", "grey", "Watch stopped.");

    public void InternalError(Exception exception) {
        Log.Error(exception, "Internal error while rebuilding");
        _output.Write("note", "cyan", "Please report this at https://github.com/glomdom/RobloxCS. Watching continues.");
    }

    public void Update(SessionUpdate update, TimeSpan elapsed) {
        if (update.Results.Count == 0 && update.DeletedOutputs.Count == 0 && update.Failure is null && update.Diagnostics.Count == 0) {
            return;
        }

        Reset();
        PrintDiagnostics(update.Diagnostics);

        foreach (var deleted in update.DeletedOutputs) {
            _output.Write("removed", "yellow", Escape(Relative(deleted)));
        }

        if (update.Failure is { } failure) {
            PrintDiagnostic(failure);
            Summary(0, 0, elapsed);
            Watching(true);

            return;
        }

        var compiled = 0;
        var failed = 0;
        foreach (var result in update.Results) {
            if (!result.Output.Ok) {
                Failure(result);
                failed++;

                continue;
            }

            PrintDiagnostics(result.Diagnostics);
            compiled++;

            if (update.Results.Count <= 3 || LoggerSetup.IsVerbose) {
                _output.Write("emitted", "green", $"{Escape(Source(result))} [grey]→ {Escape(Relative(result.Output.Value.OutPath))}[/]");
            }
        }

        Summary(compiled, failed, elapsed, update.DeletedOutputs.Count);
        Watching();
    }

    public string Relative(string path) => Path.GetRelativePath(_root, path);

    public static string Elapsed(TimeSpan elapsed) => elapsed.TotalSeconds >= 1 ? $"{elapsed.TotalSeconds:0.0} s" : $"{(int)elapsed.TotalMilliseconds} ms";

    private string Source(DocumentResult result) => result.Document.FilePath is { } path ? Relative(path) : result.Document.Name;

    private void Reset() {
        _seen.Clear();
        _warnings = 0;
        _errors = 0;
    }

    private static string Plural(int count, string word) => count == 1 ? word : word + "s";

    private static string Escape(string text) => Markup.Escape(text);
}