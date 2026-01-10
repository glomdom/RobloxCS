#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LiveSyncService", RobloxNativeType.Service)]
public static partial class LiveSyncService {
    public static bool HasSyncedInstances { get; set; } = default!;
    public static LuaTuple GetSyncState() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<Instance> SyncStatusChanged { get; private set; } = null!;
}
