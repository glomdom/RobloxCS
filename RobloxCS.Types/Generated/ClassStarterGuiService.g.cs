#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StarterGui", RobloxNativeType.Service)]
public static partial class StarterGuiService {
    public static Enums.RtlTextSupport RtlTextSupport { get; set; } = default!;
    public static Enums.ScreenOrientation ScreenOrientation { get; set; } = default!;
    public static bool ShowDevelopmentGui { get; set; } = default!;
    public static Enums.VirtualCursorMode VirtualCursorMode { get; set; } = default!;
    public static bool GetCoreGuiEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetCore() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetCoreGuiEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static object GetCore() => ThrowHelper.ThrowTranspiledMethod();
}
