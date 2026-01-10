#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DebuggerManager", RobloxNativeType.Service)]
public static partial class DebuggerManagerService {
    public static bool DebuggingEnabled { get; set; } = default!;
    public static Instance AddDebugger() => ThrowHelper.ThrowTranspiledMethod();
    public static List<Instance> GetDebuggers() => ThrowHelper.ThrowTranspiledMethod();
    public static void Resume() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<Instance> DebuggerAdded { get; private set; } = null!;
    public static RBXScriptSignal<Instance> DebuggerRemoved { get; private set; } = null!;
}
