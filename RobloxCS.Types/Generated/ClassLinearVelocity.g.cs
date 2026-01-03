#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LinearVelocity", RobloxNativeType.Instance)]
public partial class LinearVelocity : Constraint  {
    public Enums.ForceLimitMode ForceLimitMode { get; } = default!;
    public bool ForceLimitsEnabled { get; } = default!;
    public Vector3 LineDirection { get; } = default!;
    public float LineVelocity { get; } = default!;
    public Vector3 MaxAxesForce { get; } = default!;
    public float MaxForce { get; } = default!;
    public Vector2 MaxPlanarAxesForce { get; } = default!;
    public Vector2 PlaneVelocity { get; } = default!;
    public Vector3 PrimaryTangentAxis { get; } = default!;
    public bool ReactionForceEnabled { get; } = default!;
    public Enums.ActuatorRelativeTo RelativeTo { get; } = default!;
    public Vector3 SecondaryTangentAxis { get; } = default!;
    public Vector3 VectorVelocity { get; } = default!;
    public Enums.VelocityConstraintMode VelocityConstraintMode { get; } = default!;
}
