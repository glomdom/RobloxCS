#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VelocityMotor", RobloxNativeType.Instance)]
public partial class VelocityMotor : JointInstance  {
    public float CurrentAngle { get; } = default!;
    public float DesiredAngle { get; } = default!;
    public Hole Hole { get; } = default!;
    public float MaxVelocity { get; } = default!;
}
