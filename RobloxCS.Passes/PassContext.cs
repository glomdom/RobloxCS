using RobloxCS.Common.Diagnostics;

namespace RobloxCS.Passes;

/// <summary>
/// State shared by every pass in a single <see cref="PassManager.Run"/>.
/// </summary>
public sealed class PassContext {
    public DiagnosticBag Diagnostics { get; }

    public PassContext(DiagnosticBag diagnostics) {
        Diagnostics = diagnostics;
    }
}
