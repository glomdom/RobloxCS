#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Lighting", RobloxNativeType.Service)]
public static partial class LightingService {
    public static Color3 Ambient { get; } = default!;
    public static float Brightness { get; } = default!;
    public static float ClockTime { get; } = default!;
    public static Color3 ColorShift_Bottom { get; } = default!;
    public static Color3 ColorShift_Top { get; } = default!;
    public static float EnvironmentDiffuseScale { get; } = default!;
    public static float EnvironmentSpecularScale { get; } = default!;
    public static float ExposureCompensation { get; } = default!;
    public static Enums.RolloutState ExtendLightRangeTo120 { get; } = default!;
    public static Color3 FogColor { get; } = default!;
    public static float FogEnd { get; } = default!;
    public static float FogStart { get; } = default!;
    public static float GeographicLatitude { get; } = default!;
    public static bool GlobalShadows { get; } = default!;
    public static Enums.LightingStyle LightingStyle { get; } = default!;
    public static Color3 OutdoorAmbient { get; } = default!;
    public static bool PrioritizeLightingQuality { get; } = default!;
    public static float ShadowSoftness { get; } = default!;
    public static string TimeOfDay { get; } = default!;
    public static double GetMinutesAfterMidnight() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Vector3 GetMoonDirection() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static float GetMoonPhase() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Vector3 GetSunDirection() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetMinutesAfterMidnight() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal<bool> LightingChanged { get; private set; } = null!;
}
