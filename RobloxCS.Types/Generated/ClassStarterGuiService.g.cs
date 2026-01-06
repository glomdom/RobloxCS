#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StarterGui", RobloxNativeType.Service)]
public static partial class StarterGuiService {
    public static Enums.RtlTextSupport RtlTextSupport { get; set; } = default!;
    public static Enums.ScreenOrientation ScreenOrientation { get; set; } = default!;
    public static bool ShowDevelopmentGui { get; set; } = default!;
    public static Enums.VirtualCursorMode VirtualCursorMode { get; set; } = default!;
    public static bool GetCoreGuiEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetCore() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetCoreGuiEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static object GetCore() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
