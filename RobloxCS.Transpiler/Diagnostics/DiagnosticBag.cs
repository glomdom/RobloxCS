using Microsoft.CodeAnalysis;
using RobloxCS.HIR.Statements;

namespace RobloxCS.Transpiler.Diagnostics;

public sealed record Diagnostic(DiagnosticId Id, string Message, DiagnosticSeverity Severity, Location Location);

public sealed class DiagnosticBag {
    public IReadOnlyList<Diagnostic> All => _diagnostics;
    public bool HasError => _diagnostics.Any(x => x.Severity == DiagnosticSeverity.Error);

    private readonly List<Diagnostic> _diagnostics = [];

    public List<string> RenderMarkup() {
        var result = new List<string>();
        
        foreach (var diag in _diagnostics) {
            var id = diag.Id != DiagnosticId.Sentinel ? $" [yellow]{diag.Id.Format()}[/]" : string.Empty;
            var line = diag.Location.GetLineSpan();
            var location = diag.Location != Location.None ? $"{Path.GetFullPath(line.Path)}:{line.StartLinePosition.Line}:{line.StartLinePosition.Character} " : string.Empty;
            
            result.Add($"{location}[{diag.Severity.FormatColor()}]{diag.Severity.FormatName()}[/]: {diag.Message}{id}");
        }

        return result;
    }

    public void Warning(string message, Location location) {
        var diag = new Diagnostic(DiagnosticId.Sentinel, message, DiagnosticSeverity.Warn, location);
        
        _diagnostics.Add(diag);
    }
    
    public void Information(string message, Location location) {
        var diag = new Diagnostic(DiagnosticId.Sentinel, message, DiagnosticSeverity.Info, location);
        
        _diagnostics.Add(diag);
    }
    
    public void Trace(string message, Location location) {
        var diag = new Diagnostic(DiagnosticId.Sentinel, message, DiagnosticSeverity.Trace, location);
        
        _diagnostics.Add(diag);
    }

    public HirErrorStatement UnsupportedStatement(string message, Location location) {
        var diag = new Diagnostic(DiagnosticId.UnsupportedSyntax, message, DiagnosticSeverity.Error, location);

        _diagnostics.Add(diag);

        return new HirErrorStatement {
            Location = location,
        };
    }
}