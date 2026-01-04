#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LiveSyncService", RobloxNativeType.Service)]
public static partial class LiveSyncService {
    public static bool HasSyncedInstances { get; } = default!;
    public static LuaTuple GetSyncState() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<Instance> SyncStatusChanged { get; private set; } = null!;
}
