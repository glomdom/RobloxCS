#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StarterPlayer", RobloxNativeType.Service)]
public static partial class StarterPlayerService {
    public static bool AutoJumpEnabled { get; } = default!;
    public static Enums.RolloutState AvatarJointUpgrade { get; } = default!;
    public static float CameraMaxZoomDistance { get; } = default!;
    public static float CameraMinZoomDistance { get; } = default!;
    public static Enums.CameraMode CameraMode { get; } = default!;
    public static bool CharacterBreakJointsOnDeath { get; } = default!;
    public static float CharacterJumpHeight { get; } = default!;
    public static float CharacterJumpPower { get; } = default!;
    public static float CharacterMaxSlopeAngle { get; } = default!;
    public static bool CharacterUseJumpPower { get; } = default!;
    public static float CharacterWalkSpeed { get; } = default!;
    public static bool ClassicDeath { get; } = default!;
    public static bool CreateDefaultPlayerModule { get; } = default!;
    public static Enums.DevCameraOcclusionMode DevCameraOcclusionMode { get; } = default!;
    public static Enums.DevComputerCameraMovementMode DevComputerCameraMovementMode { get; } = default!;
    public static Enums.DevComputerMovementMode DevComputerMovementMode { get; } = default!;
    public static Enums.DevTouchCameraMovementMode DevTouchCameraMovementMode { get; } = default!;
    public static Enums.DevTouchMovementMode DevTouchMovementMode { get; } = default!;
    public static Enums.LoadDynamicHeads EnableDynamicHeads { get; } = default!;
    public static bool EnableMouseLockOption { get; } = default!;
    public static float HealthDisplayDistance { get; } = default!;
    public static bool LoadCharacterAppearance { get; } = default!;
    public static Enums.LoadCharacterLayeredClothing LoadCharacterLayeredClothing { get; } = default!;
    public static Enums.CharacterControlMode LuaCharacterController { get; } = default!;
    public static float NameDisplayDistance { get; } = default!;
    public static bool UserEmotesEnabled { get; } = default!;
}
