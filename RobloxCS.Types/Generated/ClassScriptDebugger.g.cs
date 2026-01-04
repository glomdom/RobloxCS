#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScriptDebugger", RobloxNativeType.Instance)]
public partial class ScriptDebugger : Instance  {
    public int CurrentLine { get; } = default!;
    public bool IsDebugging { get; } = default!;
    public bool IsPaused { get; } = default!;
    public Instance Script { get; } = default!;
    public Instance AddWatch() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetBreakpoints() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<object, object> GetGlobals() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<object, object> GetLocals() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetStack() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<object, object> GetUpvalues() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetWatchValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<Instance> GetWatches() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance SetBreakpoint() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetGlobal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetLocal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetUpvalue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Instance> BreakpointAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> BreakpointRemoved { get; private set; } = null!;
    public RBXScriptSignal<int, Enums.BreakReason> EncounteredBreak { get; private set; } = null!;
    public RBXScriptSignal Resuming { get; private set; } = null!;
    public RBXScriptSignal<Instance> WatchAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> WatchRemoved { get; private set; } = null!;
}
