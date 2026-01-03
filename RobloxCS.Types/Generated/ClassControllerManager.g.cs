#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ControllerManager", RobloxNativeType.Instance)]
public partial class ControllerManager : Instance  {
    public ControllerBase ActiveController { get; } = default!;
    public float BaseMoveSpeed { get; } = default!;
    public float BaseTurnSpeed { get; } = default!;
    public ControllerSensor ClimbSensor { get; } = default!;
    public Vector3 FacingDirection { get; } = default!;
    public ControllerSensor GroundSensor { get; } = default!;
    public Vector3 MovingDirection { get; } = default!;
    public BasePart RootPart { get; } = default!;
    public Vector3 UpDirection { get; } = default!;
}
