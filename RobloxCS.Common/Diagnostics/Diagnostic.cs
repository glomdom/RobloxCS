using Microsoft.CodeAnalysis;

namespace RobloxCS.Common.Diagnostics;

public sealed record Diagnostic(string Id, Severity Severity, string Message, Location? Location) {
    public static Diagnostic Error(string id, string message, Location? location = null) => new(id, Severity.Error, message, location);

    public static Diagnostic FromRoslyn(Microsoft.CodeAnalysis.Diagnostic d) => new(
        d.Id,
        d.Severity == DiagnosticSeverity.Error ? Severity.Error : Severity.Warning,
        d.GetMessage(),
        d.Location
    );

    public string Render() {
        var severity = Severity == Severity.Error ? "error" : "warning";

        if (Location is { IsInSource: true }) {
            var span = Location.GetLineSpan();
            var line = span.StartLinePosition.Line + 1;
            var col = span.StartLinePosition.Character + 1;

            return $"{span.Path}({line},{col}): {severity} {Id}: {Message}";
        }

        return $"{severity} {Id}: {Message}";
    }
}