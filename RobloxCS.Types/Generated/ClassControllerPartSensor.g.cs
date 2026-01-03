#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ControllerPartSensor", RobloxNativeType.Instance)]
public partial class ControllerPartSensor : ControllerSensor  {
    public CFrame HitFrame { get; } = default!;
    public Vector3 HitNormal { get; } = default!;
    public float SearchDistance { get; } = default!;
    public BasePart SensedPart { get; } = default!;
    public Enums.SensorMode SensorMode { get; } = default!;
}
