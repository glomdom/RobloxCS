#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UserInputService", RobloxNativeType.Service)]
public static partial class UserInputService {
    public static bool AccelerometerEnabled { get; } = default!;
    public static bool GamepadEnabled { get; } = default!;
    public static bool GyroscopeEnabled { get; } = default!;
    public static bool KeyboardEnabled { get; } = default!;
    public static Enums.MouseBehavior MouseBehavior { get; } = default!;
    public static float MouseDeltaSensitivity { get; } = default!;
    public static bool MouseEnabled { get; } = default!;
    public static string MouseIcon { get; } = default!;
    public static string MouseIconContent { get; } = default!;
    public static bool MouseIconEnabled { get; } = default!;
    public static Vector2 OnScreenKeyboardPosition { get; } = default!;
    public static Vector2 OnScreenKeyboardSize { get; } = default!;
    public static bool OnScreenKeyboardVisible { get; } = default!;
    public static Enums.PreferredInput PreferredInput { get; } = default!;
    public static bool TouchEnabled { get; } = default!;
    public static bool VREnabled { get; } = default!;
    public static bool GamepadSupports() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetConnectedGamepads() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static InputObject GetDeviceAcceleration() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static InputObject GetDeviceGravity() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple GetDeviceRotation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static TextBox GetFocusedTextBox() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool GetGamepadConnected() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<Instance> GetGamepadState() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GetImageForKeyCode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<Instance> GetKeysPressed() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Enums.UserInputType GetLastInputType() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<Instance> GetMouseButtonsPressed() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Vector2 GetMouseDelta() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static Vector2 GetMouseLocation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetNavigationGamepads() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string GetStringForKeyCode() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> GetSupportedGamepadKeyCodes() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsGamepadButtonDown() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsKeyDown() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsMouseButtonPressed() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool IsNavigationGamepad() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RecenterUserHeadCFrame() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetNavigationGamepad() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
