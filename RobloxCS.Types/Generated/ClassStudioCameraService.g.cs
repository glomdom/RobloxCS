#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StudioCameraService", RobloxNativeType.Service)]
public static partial class StudioCameraService {
    public static bool LockCameraSpeed { get; set; } = default!;
    public static bool LoggingEnabled { get; set; } = default!;
    public static RBXScriptSignal<float> ShowCameraSpeed { get; private set; } = null!;
}
