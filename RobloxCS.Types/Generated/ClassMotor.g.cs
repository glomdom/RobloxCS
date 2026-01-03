#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Motor", RobloxNativeType.Instance)]
public partial class Motor : JointInstance  {
    public float CurrentAngle { get; } = default!;
    public float DesiredAngle { get; } = default!;
    public float MaxVelocity { get; } = default!;
    public void SetDesiredAngle() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
