using RobloxCS.Common.Diagnostics;
using Spectre.Console;

namespace RobloxCS.CLI;

/// <summary>
/// Coloured console rendering for <see cref="Diagnostic"/>. Everything user-supplied is escaped.
/// </summary>
internal static class DiagnosticMarkup {
    public static void Print(IAnsiConsole console, Diagnostic diagnostic) {
        console.MarkupLine(Render(diagnostic));
    }

    public static void PrintAll(IAnsiConsole console, IEnumerable<Diagnostic> diagnostics) {
        foreach (var diagnostic in diagnostics) {
            Print(console, diagnostic);
        }
    }

    private static string Render(Diagnostic diagnostic) {
        var location = diagnostic.FormatLocation() is { } loc ? $"{Markup.Escape(loc)}: " : string.Empty;
        var id = diagnostic.Id is not null ? $" [yellow]{Markup.Escape(diagnostic.Id)}[/]" : string.Empty;
        var severity = $"[{Color(diagnostic.Severity)}]{diagnostic.Severity.FormatName()}[/]";

        return $"{location}{severity}{id}: {Markup.Escape(diagnostic.Message)}";
    }

    private static string Color(Severity severity) => severity switch {
        Severity.Error => "red",
        Severity.Warning => "yellow",
        Severity.Note => "cyan",
        Severity.Info => "blue",
        Severity.Debug => "white",
        Severity.Trace => "gray",

        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null),
    };
}
