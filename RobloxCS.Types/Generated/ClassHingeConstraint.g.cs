#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HingeConstraint", RobloxNativeType.Instance)]
public partial class HingeConstraint : Constraint  {
    public Enums.ActuatorType ActuatorType { get; } = default!;
    public float AngularResponsiveness { get; } = default!;
    public float AngularSpeed { get; } = default!;
    public float AngularVelocity { get; } = default!;
    public float CurrentAngle { get; } = default!;
    public bool LimitsEnabled { get; } = default!;
    public float LowerAngle { get; } = default!;
    public float MotorMaxAcceleration { get; } = default!;
    public float MotorMaxTorque { get; } = default!;
    public float Radius { get; } = default!;
    public float Restitution { get; } = default!;
    public float ServoMaxTorque { get; } = default!;
    public float TargetAngle { get; } = default!;
    public float UpperAngle { get; } = default!;
}
