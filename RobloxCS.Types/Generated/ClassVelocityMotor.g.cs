#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VelocityMotor", RobloxNativeType.Instance)]
public partial class VelocityMotor : JointInstance  {
    public float CurrentAngle { get; set; } = default!;
    public float DesiredAngle { get; set; } = default!;
    public Hole Hole { get; set; } = default!;
    public float MaxVelocity { get; set; } = default!;
}
