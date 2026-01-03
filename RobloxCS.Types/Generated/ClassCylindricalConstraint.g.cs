#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CylindricalConstraint", RobloxNativeType.Instance)]
public partial class CylindricalConstraint : SlidingBallConstraint  {
    public Enums.ActuatorType AngularActuatorType { get; } = default!;
    public bool AngularLimitsEnabled { get; } = default!;
    public float AngularResponsiveness { get; } = default!;
    public float AngularRestitution { get; } = default!;
    public float AngularSpeed { get; } = default!;
    public float AngularVelocity { get; } = default!;
    public float CurrentAngle { get; } = default!;
    public float InclinationAngle { get; } = default!;
    public float LowerAngle { get; } = default!;
    public float MotorMaxAngularAcceleration { get; } = default!;
    public float MotorMaxTorque { get; } = default!;
    public bool RotationAxisVisible { get; } = default!;
    public float ServoMaxTorque { get; } = default!;
    public float TargetAngle { get; } = default!;
    public float UpperAngle { get; } = default!;
    public Vector3 WorldRotationAxis { get; } = default!;
}
