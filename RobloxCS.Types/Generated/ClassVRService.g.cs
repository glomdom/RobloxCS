#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VRService", RobloxNativeType.Service)]
public static partial class VRService {
    public static Enums.VRScaling AutomaticScaling { get; } = default!;
    public static bool AvatarGestures { get; } = default!;
    public static Enums.VRControllerModelMode ControllerModels { get; } = default!;
    public static bool FadeOutViewOnCollision { get; } = default!;
    public static Enums.UserCFrame GuiInputUserCFrame { get; } = default!;
    public static Enums.VRLaserPointerMode LaserPointer { get; } = default!;
    public static bool ThirdPersonFollowCamEnabled { get; } = default!;
    public static bool VREnabled { get; } = default!;
    public static Enums.VRTouchpadMode GetTouchpadMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static CFrame GetUserCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetUserCFrameEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RecenterUserHeadCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RequestNavigation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetTouchpadMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
