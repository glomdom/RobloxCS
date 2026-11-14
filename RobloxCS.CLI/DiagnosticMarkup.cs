using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using Spectre.Console;
using LocationKind = Microsoft.CodeAnalysis.LocationKind;

namespace RobloxCS.CLI;

internal static class DiagnosticMarkup {
    public static void Print(IAnsiConsole console, Diagnostic diagnostic, string? root = null) {
        var hasSource = diagnostic.Location?.Kind is LocationKind.SourceFile or LocationKind.ExternalFile;
        var location = hasSource ? diagnostic.Location!.GetLineSpan() : default;
        var path = hasSource
            ? root is null || !Path.IsPathRooted(location.Path) ? location.Path : Path.GetRelativePath(root, location.Path)
            : null;
        var where = string.IsNullOrEmpty(path) ? string.Empty : $"[grey]{Markup.Escape(path)}({location.StartLinePosition.Line + 1},{location.StartLinePosition.Character + 1})[/] ";
        var id = diagnostic.Id is null ? string.Empty : $"[yellow]{Markup.Escape(diagnostic.Id)}[/] ";

        new TerminalOutput(console).Write(diagnostic.Severity.FormatName(), Color(diagnostic.Severity), $"{where}{id}{Markup.Escape(diagnostic.Message)}");
    }

    public static void PrintAll(IAnsiConsole console, IEnumerable<Diagnostic> diagnostics) {
        foreach (var diagnostic in diagnostics) {
            Print(console, diagnostic);
        }
    }

    private static string Color(Severity severity) => severity switch {
        Severity.Error => "red",
        Severity.Warning => "yellow",
        Severity.Note => "cyan",
        Severity.Info => "blue",
        Severity.Debug => "grey",
        Severity.Trace => "grey",

        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null),
    };
}
