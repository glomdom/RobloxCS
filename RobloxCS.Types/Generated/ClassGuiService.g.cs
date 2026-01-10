#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiService", RobloxNativeType.Service)]
public static partial class GuiService {
    public static bool AutoSelectGuiEnabled { get; set; } = default!;
    public static bool GuiNavigationEnabled { get; set; } = default!;
    public static bool MenuIsOpen { get; set; } = default!;
    public static Enums.PreferredTextSize PreferredTextSize { get; set; } = default!;
    public static GuiObject SelectedObject { get; set; } = default!;
    public static Rect TopbarInset { get; set; } = default!;
    public static bool TouchControlsEnabled { get; set; } = default!;
    public static Enums.DisplaySize ViewportDisplaySize { get; set; } = default!;
    public static void CloseInspectMenu() => ThrowHelper.ThrowTranspiledMethod();
    public static bool DismissNotification() => ThrowHelper.ThrowTranspiledMethod();
    public static bool GetEmotesMenuOpen() => ThrowHelper.ThrowTranspiledMethod();
    public static bool GetGameplayPausedNotificationEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static LuaTuple GetGuiInset() => ThrowHelper.ThrowTranspiledMethod();
    public static Rect GetInsetArea() => ThrowHelper.ThrowTranspiledMethod();
    public static bool GetInspectMenuEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static void InspectPlayerFromHumanoidDescription() => ThrowHelper.ThrowTranspiledMethod();
    public static void InspectPlayerFromUserId() => ThrowHelper.ThrowTranspiledMethod();
    public static bool IsTenFootInterface() => ThrowHelper.ThrowTranspiledMethod();
    public static void Select() => ThrowHelper.ThrowTranspiledMethod();
    public static string SendNotification() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetEmotesMenuOpen() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetGameplayPausedNotificationEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static void SetInspectMenuEnabled() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal MenuClosed { get; private set; } = null!;
    public static RBXScriptSignal MenuOpened { get; private set; } = null!;
}
