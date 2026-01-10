#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UserGameSettings", RobloxNativeType.Service)]
public static partial class UserGameSettingsService {
    public static Enums.ComputerCameraMovementMode ComputerCameraMovementMode { get; set; } = default!;
    public static Enums.ComputerMovementMode ComputerMovementMode { get; set; } = default!;
    public static Enums.ControlMode ControlMode { get; set; } = default!;
    public static float GamepadCameraSensitivity { get; set; } = default!;
    public static float MouseSensitivity { get; set; } = default!;
    public static int RCCProfilerRecordFrameRate { get; set; } = default!;
    public static int RCCProfilerRecordTimeFrame { get; set; } = default!;
    public static Enums.RotationType RotationType { get; set; } = default!;
    public static Enums.SavedQualitySetting SavedQualityLevel { get; set; } = default!;
    public static Enums.TouchCameraMovementMode TouchCameraMovementMode { get; set; } = default!;
    public static Enums.TouchMovementMode TouchMovementMode { get; set; } = default!;
    public static bool VRSmoothRotationEnabled { get; } = default!;
    public static bool VignetteEnabled { get; } = default!;
    public static int GetCameraYInvertValue() => ThrowHelper.ThrowTranspiledMethod();
    public static bool GetOnboardingCompleted() => ThrowHelper.ThrowTranspiledMethod();
    public static bool InFullScreen() => ThrowHelper.ThrowTranspiledMethod();
    public static bool InStudioMode() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetCameraYInvertVisible() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetGamepadCameraSensitivityVisible() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetOnboardingCompleted() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<bool> FullscreenChanged { get; private set; } = null!;
    public static RBXScriptSignal<bool> StudioModeChanged { get; private set; } = null!;
}
