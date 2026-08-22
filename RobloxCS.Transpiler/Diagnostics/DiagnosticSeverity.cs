namespace RobloxCS.Transpiler.Diagnostics;

public enum DiagnosticSeverity {
    Error,
    Warn,
    Note,
    Info,
    Debug,
    Trace,
}

public static class DiagnosticSeverityExtensions {
    public static string FormatColor(this DiagnosticSeverity severity) => severity switch {
        DiagnosticSeverity.Error => "red",
        DiagnosticSeverity.Warn => "yellow",
        DiagnosticSeverity.Note => "cyan",
        DiagnosticSeverity.Info => "blue",
        DiagnosticSeverity.Debug => "white",
        DiagnosticSeverity.Trace => "gray",

        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
    };
    
    public static string FormatName(this DiagnosticSeverity severity) => severity switch {
        DiagnosticSeverity.Error => "error",
        DiagnosticSeverity.Warn => "warn",
        DiagnosticSeverity.Note => "note",
        DiagnosticSeverity.Info => "info",
        DiagnosticSeverity.Debug => "debug",
        DiagnosticSeverity.Trace => "trace",

        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null)
    }; 
}