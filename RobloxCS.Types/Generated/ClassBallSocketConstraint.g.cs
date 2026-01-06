#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BallSocketConstraint", RobloxNativeType.Instance)]
public partial class BallSocketConstraint : Constraint  {
    public bool LimitsEnabled { get; set; } = default!;
    public float MaxFrictionTorque { get; set; } = default!;
    public float Radius { get; set; } = default!;
    public float Restitution { get; set; } = default!;
    public bool TwistLimitsEnabled { get; set; } = default!;
    public float TwistLowerAngle { get; set; } = default!;
    public float TwistUpperAngle { get; set; } = default!;
    public float UpperAngle { get; set; } = default!;
}
