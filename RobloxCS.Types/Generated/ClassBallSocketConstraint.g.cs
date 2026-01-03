#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BallSocketConstraint", RobloxNativeType.Instance)]
public partial class BallSocketConstraint : Constraint  {
    public bool LimitsEnabled { get; } = default!;
    public float MaxFrictionTorque { get; } = default!;
    public float Radius { get; } = default!;
    public float Restitution { get; } = default!;
    public bool TwistLimitsEnabled { get; } = default!;
    public float TwistLowerAngle { get; } = default!;
    public float TwistUpperAngle { get; } = default!;
    public float UpperAngle { get; } = default!;
}
