using RobloxCS.Common.Diagnostics;
using RobloxCS.CompilerPipeline;
using Spectre.Console;

namespace RobloxCS.CLI.Reporting;

internal sealed class BuildReporter {
    private readonly IAnsiConsole _console;
    private readonly string _root;

    public BuildReporter(IAnsiConsole console, string root) {
        _console = console;
        _root = root;
    }

    public void Status(string markup) {
        _console.MarkupLine($"[grey]{DateTime.Now:HH:mm:ss}[/]  {markup}");
    }

    public void PrintDiagnostics(IEnumerable<Diagnostic> diagnostics) {
        DiagnosticMarkup.PrintAll(_console, diagnostics);
    }

    public void PrintDiagnostic(Diagnostic diagnostic) {
        DiagnosticMarkup.Print(_console, diagnostic);
    }

    public void Failure(DocumentResult result) {
        Status($"[red]✗[/] {Escape(result.Document.Name)}");
        PrintDiagnostics(result.Diagnostics);

        if (result.Output.Diagnostic is { } diagnostic) {
            PrintDiagnostic(diagnostic);
        }
    }

    public void Summary(int compiled, int failed, TimeSpan elapsed, string? suffix = null) {
        var files = compiled == 1 ? "1 file" : $"{compiled} files";
        var failures = failed == 0 ? string.Empty : $", [red]{failed} failed[/]";
        var verb = failed == 0 ? "[green]built[/]" : "built";
        var tail = suffix is null ? string.Empty : $", [grey]{suffix}[/]";

        Status($"{verb} {files}{failures} in {Elapsed(elapsed)}{tail}");
    }

    public void Update(SessionUpdate update, TimeSpan elapsed) {
        if (update.Results.Count == 0 && update.DeletedOutputs.Count == 0 && update.Failure is null && update.Diagnostics.Count == 0) {
            return;
        }

        PrintDiagnostics(update.Diagnostics);

        if (update.Failure is { } failure) {
            PrintDiagnostic(failure);

            return;
        }

        foreach (var deleted in update.DeletedOutputs) {
            Status($"[yellow]-[/] [grey]{Escape(Relative(deleted))}[/]");
        }

        var succeeded = update.Results.Where(result => result.Output.Ok).ToList();
        var failed = update.Results.Where(result => !result.Output.Ok).ToList();
        var time = $"[grey]{Elapsed(elapsed)}[/]";

        foreach (var result in succeeded) {
            PrintDiagnostics(result.Diagnostics);
        }

        if (succeeded.Count == 1) {
            var only = succeeded[0];

            Status($"[green]✓[/] {Escape(only.Document.Name)} [grey]→ {Escape(Relative(only.Output.Value!.OutPath))}[/]  {time}");
        } else if (succeeded.Count > 1) {
            var names = string.Join(", ", succeeded.Select(result => Escape(result.Document.Name)));

            Status($"[green]✓[/] {names}  {time}");
        }

        foreach (var result in failed) {
            Failure(result);
        }
    }

    public string Relative(string path) => Path.GetRelativePath(_root, path);

    public static string Elapsed(TimeSpan elapsed) {
        return elapsed.TotalSeconds >= 1 ? $"{elapsed.TotalSeconds:0.0} s" : $"{(int)elapsed.TotalMilliseconds} ms";
    }

    private static string Escape(string text) => Markup.Escape(text);
}