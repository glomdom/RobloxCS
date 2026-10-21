using Microsoft.CodeAnalysis;

namespace RobloxCS.Common.Diagnostics;

public sealed class DiagnosticBag {
    public IReadOnlyList<Diagnostic> All => _diagnostics;
    public bool HasErrors => _diagnostics.Any(d => d.IsError);

    private readonly List<Diagnostic> _diagnostics = [];

    public void Add(Diagnostic diagnostic) => _diagnostics.Add(diagnostic);
    public void AddRange(IEnumerable<Diagnostic> diagnostics) => _diagnostics.AddRange(diagnostics);

    public void Error(string id, string message, Location? location) => Add(new Diagnostic(id, Severity.Error, message, location));
    public void Warning(string? id, string message, Location? location) => Add(new Diagnostic(id, Severity.Warning, message, location));
    public void Info(string message, Location? location) => Add(new Diagnostic(null, Severity.Info, message, location));
    public void Trace(string message, Location? location) => Add(new Diagnostic(null, Severity.Trace, message, location));
}
