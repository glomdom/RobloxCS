namespace RobloxCS.Common.Diagnostics;

public enum Severity {
    Error,
    Warning,
    Note,
    Info,
    Debug,
    Trace,
}

public static class SeverityExtensions {
    public static string FormatName(this Severity severity) => severity switch {
        Severity.Error => "error",
        Severity.Warning => "warning",
        Severity.Note => "note",
        Severity.Info => "info",
        Severity.Debug => "debug",
        Severity.Trace => "trace",

        _ => throw new ArgumentOutOfRangeException(nameof(severity), severity, null),
    };
}
