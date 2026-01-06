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
    public static int GetCameraYInvertValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetOnboardingCompleted() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool InFullScreen() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool InStudioMode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetCameraYInvertVisible() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetGamepadCameraSensitivityVisible() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetOnboardingCompleted() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<bool> FullscreenChanged { get; private set; } = null!;
    public static RBXScriptSignal<bool> StudioModeChanged { get; private set; } = null!;
}
