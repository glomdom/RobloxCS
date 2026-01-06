#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VRService", RobloxNativeType.Service)]
public static partial class VRService {
    public static Enums.VRScaling AutomaticScaling { get; set; } = default!;
    public static bool AvatarGestures { get; set; } = default!;
    public static Enums.VRControllerModelMode ControllerModels { get; set; } = default!;
    public static bool FadeOutViewOnCollision { get; set; } = default!;
    public static Enums.UserCFrame GuiInputUserCFrame { get; set; } = default!;
    public static Enums.VRLaserPointerMode LaserPointer { get; set; } = default!;
    public static bool ThirdPersonFollowCamEnabled { get; set; } = default!;
    public static bool VREnabled { get; set; } = default!;
    public static Enums.VRTouchpadMode GetTouchpadMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static CFrame GetUserCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetUserCFrameEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RecenterUserHeadCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RequestNavigation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetTouchpadMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<CFrame, Enums.UserCFrame> NavigationRequested { get; private set; } = null!;
    public static RBXScriptSignal<Enums.VRTouchpad, Enums.VRTouchpadMode> TouchpadModeChanged { get; private set; } = null!;
    public static RBXScriptSignal<Enums.UserCFrame, CFrame> UserCFrameChanged { get; private set; } = null!;
    public static RBXScriptSignal<Enums.UserCFrame, bool> UserCFrameEnabled { get; private set; } = null!;
}
