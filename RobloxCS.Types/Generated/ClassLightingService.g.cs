#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Lighting", RobloxNativeType.Service)]
public static partial class LightingService {
    public static Color3 Ambient { get; set; } = default!;
    public static float Brightness { get; set; } = default!;
    public static float ClockTime { get; set; } = default!;
    public static Color3 ColorShift_Bottom { get; set; } = default!;
    public static Color3 ColorShift_Top { get; set; } = default!;
    public static float EnvironmentDiffuseScale { get; set; } = default!;
    public static float EnvironmentSpecularScale { get; set; } = default!;
    public static float ExposureCompensation { get; set; } = default!;
    public static Enums.RolloutState ExtendLightRangeTo120 { get; set; } = default!;
    public static Color3 FogColor { get; set; } = default!;
    public static float FogEnd { get; set; } = default!;
    public static float FogStart { get; set; } = default!;
    public static float GeographicLatitude { get; set; } = default!;
    public static bool GlobalShadows { get; set; } = default!;
    public static Enums.LightingStyle LightingStyle { get; } = default!;
    public static Color3 OutdoorAmbient { get; set; } = default!;
    public static bool PrioritizeLightingQuality { get; } = default!;
    public static float ShadowSoftness { get; set; } = default!;
    public static string TimeOfDay { get; set; } = default!;
    public static double GetMinutesAfterMidnight() => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 GetMoonDirection() => ThrowHelper.ThrowTranspiledMethod();
    public static float GetMoonPhase() => ThrowHelper.ThrowTranspiledMethod();
    public static Vector3 GetSunDirection() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetMinutesAfterMidnight() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<bool> LightingChanged { get; private set; } = null!;
}
