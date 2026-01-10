#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScriptDebugger", RobloxNativeType.Instance)]
public partial class ScriptDebugger : Instance  {
    public int CurrentLine { get; set; } = default!;
    public bool IsDebugging { get; set; } = default!;
    public bool IsPaused { get; set; } = default!;
    public Instance Script { get; set; } = default!;
    public Instance AddWatch() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetBreakpoints() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<object, object> GetGlobals() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<object, object> GetLocals() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetStack() => ThrowHelper.ThrowTranspiledMethod();
    public Dictionary<object, object> GetUpvalues() => ThrowHelper.ThrowTranspiledMethod();
    public object GetWatchValue() => ThrowHelper.ThrowTranspiledMethod();
    public List<Instance> GetWatches() => ThrowHelper.ThrowTranspiledMethod();
    public Instance SetBreakpoint() => ThrowHelper.ThrowTranspiledMethod();
    public void SetGlobal() => ThrowHelper.ThrowTranspiledMethod();
    public void SetLocal() => ThrowHelper.ThrowTranspiledMethod();
    public void SetUpvalue() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<Instance> BreakpointAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> BreakpointRemoved { get; private set; } = null!;
    public RBXScriptSignal<int, Enums.BreakReason> EncounteredBreak { get; private set; } = null!;
    public RBXScriptSignal Resuming { get; private set; } = null!;
    public RBXScriptSignal<Instance> WatchAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> WatchRemoved { get; private set; } = null!;
}
