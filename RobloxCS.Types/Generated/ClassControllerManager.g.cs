#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ControllerManager", RobloxNativeType.Instance)]
public partial class ControllerManager : Instance  {
    public ControllerBase ActiveController { get; set; } = default!;
    public float BaseMoveSpeed { get; set; } = default!;
    public float BaseTurnSpeed { get; set; } = default!;
    public ControllerSensor ClimbSensor { get; set; } = default!;
    public Vector3 FacingDirection { get; set; } = default!;
    public ControllerSensor GroundSensor { get; set; } = default!;
    public Vector3 MovingDirection { get; set; } = default!;
    public BasePart RootPart { get; set; } = default!;
    public Vector3 UpDirection { get; set; } = default!;
}
