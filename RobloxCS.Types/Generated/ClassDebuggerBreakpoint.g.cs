#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DebuggerBreakpoint", RobloxNativeType.Instance)]
public partial class DebuggerBreakpoint : Instance  {
    public string Condition { get; } = default!;
    public bool ContinueExecution { get; } = default!;
    public bool IsEnabled { get; } = default!;
    public int Line { get; } = default!;
    public string LogExpression { get; } = default!;
    public bool isContextDependentBreakpoint { get; } = default!;
}
