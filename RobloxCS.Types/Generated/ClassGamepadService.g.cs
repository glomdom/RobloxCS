#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GamepadService", RobloxNativeType.Service)]
public static partial class GamepadService {
    public static bool GamepadCursorEnabled { get; } = default!;
    public static void DisableGamepadCursor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void EnableGamepadCursor() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
