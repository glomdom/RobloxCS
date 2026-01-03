#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TorsionSpringConstraint", RobloxNativeType.Instance)]
public partial class TorsionSpringConstraint : Constraint  {
    public float Coils { get; } = default!;
    public float CurrentAngle { get; } = default!;
    public float Damping { get; } = default!;
    public bool LimitsEnabled { get; } = default!;
    public float MaxAngle { get; } = default!;
    public float MaxTorque { get; } = default!;
    public float Radius { get; } = default!;
    public float Restitution { get; } = default!;
    public float Stiffness { get; } = default!;
}
