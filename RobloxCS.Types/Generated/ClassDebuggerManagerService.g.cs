#nullable enable
namespace RobloxCS.Types;
[RobloxNative("DebuggerManager", RobloxNativeType.Service)]
public static partial class DebuggerManagerService {
    public static bool DebuggingEnabled { get; } = default!;
    public static Instance AddDebugger() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<Instance> GetDebuggers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Resume() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
