#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GamepadService", RobloxNativeType.Service)]
public static partial class GamepadService {
    public static bool GamepadCursorEnabled { get; } = default!;
    public static void DisableGamepadCursor() => ThrowHelper.ThrowTranspiledMethod();
    public static void EnableGamepadCursor() => ThrowHelper.ThrowTranspiledMethod();
}
