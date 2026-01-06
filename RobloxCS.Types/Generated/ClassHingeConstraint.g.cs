#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HingeConstraint", RobloxNativeType.Instance)]
public partial class HingeConstraint : Constraint  {
    public Enums.ActuatorType ActuatorType { get; set; } = default!;
    public float AngularResponsiveness { get; set; } = default!;
    public float AngularSpeed { get; set; } = default!;
    public float AngularVelocity { get; set; } = default!;
    public float CurrentAngle { get; set; } = default!;
    public bool LimitsEnabled { get; set; } = default!;
    public float LowerAngle { get; set; } = default!;
    public float MotorMaxAcceleration { get; set; } = default!;
    public float MotorMaxTorque { get; set; } = default!;
    public float Radius { get; set; } = default!;
    public float Restitution { get; set; } = default!;
    public float ServoMaxTorque { get; set; } = default!;
    public float TargetAngle { get; set; } = default!;
    public float UpperAngle { get; set; } = default!;
}
