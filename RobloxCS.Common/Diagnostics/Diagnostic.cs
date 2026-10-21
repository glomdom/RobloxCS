using Microsoft.CodeAnalysis;

namespace RobloxCS.Common.Diagnostics;

/// <param name="Id">One of <see cref="DiagnosticId"/>, or <c>null</c> for untagged diagnostics.</param>
public sealed record Diagnostic(string? Id, Severity Severity, string Message, Location? Location) {
    public static Diagnostic Error(string id, string message, Location? location = null) => new(id, Severity.Error, message, location);
    public static Diagnostic Warning(string? id, string message, Location? location = null) => new(id, Severity.Warning, message, location);

    public static Diagnostic FromRoslyn(Microsoft.CodeAnalysis.Diagnostic d) => new(
        d.Id,
        d.Severity == DiagnosticSeverity.Error ? Severity.Error : Severity.Warning,
        d.GetMessage(),
        d.Location
    );

    public bool IsError => Severity == Severity.Error;

    /// <summary>
    /// Formats the location as <c>path(line,col)</c> with 1-based positions, or <c>null</c> when not in source.
    /// </summary>
    public string? FormatLocation() {
        if (Location is not { IsInSource: true }) {
            return null;
        }

        var span = Location.GetLineSpan();

        return $"{span.Path}({span.StartLinePosition.Line + 1},{span.StartLinePosition.Character + 1})";
    }

    public string Render() {
        var location = FormatLocation() is { } loc ? $"{loc}: " : string.Empty;
        var id = Id is not null ? $" {Id}" : string.Empty;

        return $"{location}{Severity.FormatName()}{id}: {Message}";
    }
}
