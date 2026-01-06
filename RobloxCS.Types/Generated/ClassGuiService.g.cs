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
    public static void CloseInspectMenu() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool DismissNotification() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetEmotesMenuOpen() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetGameplayPausedNotificationEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple GetGuiInset() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetInspectMenuEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void InspectPlayerFromHumanoidDescription() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void InspectPlayerFromUserId() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsTenFootInterface() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void Select() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string SendNotification() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetEmotesMenuOpen() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetGameplayPausedNotificationEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetInspectMenuEnabled() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal MenuClosed { get; private set; } = null!;
    public static RBXScriptSignal MenuOpened { get; private set; } = null!;
}
