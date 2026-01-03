#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SlidingBallConstraint", RobloxNativeType.Instance)]
public partial class SlidingBallConstraint : Constraint  {
    public Enums.ActuatorType ActuatorType { get; } = default!;
    public float CurrentPosition { get; } = default!;
    public bool LimitsEnabled { get; } = default!;
    public float LinearResponsiveness { get; } = default!;
    public float LowerLimit { get; } = default!;
    public float MotorMaxAcceleration { get; } = default!;
    public float MotorMaxForce { get; } = default!;
    public float Restitution { get; } = default!;
    public float ServoMaxForce { get; } = default!;
    public float Size { get; } = default!;
    public float Speed { get; } = default!;
    public float TargetPosition { get; } = default!;
    public float UpperLimit { get; } = default!;
    public float Velocity { get; } = default!;
}
