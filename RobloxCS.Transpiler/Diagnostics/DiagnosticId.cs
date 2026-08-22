namespace RobloxCS.Transpiler.Diagnostics;

public enum DiagnosticId {
    UnsupportedSyntax = 100,
    
    /// <summary>
    /// Sentinel tag which doesn't get rendered.
    /// </summary>
    Sentinel,
}

public static class DiagnosticIdExtensions {
    public static string Format(this DiagnosticId id) => $"RCS{(int)id:D4}";
}