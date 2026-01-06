#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LinearVelocity", RobloxNativeType.Instance)]
public partial class LinearVelocity : Constraint  {
    public Enums.ForceLimitMode ForceLimitMode { get; set; } = default!;
    public bool ForceLimitsEnabled { get; set; } = default!;
    public Vector3 LineDirection { get; set; } = default!;
    public float LineVelocity { get; set; } = default!;
    public Vector3 MaxAxesForce { get; set; } = default!;
    public float MaxForce { get; set; } = default!;
    public Vector2 MaxPlanarAxesForce { get; set; } = default!;
    public Vector2 PlaneVelocity { get; set; } = default!;
    public Vector3 PrimaryTangentAxis { get; set; } = default!;
    public bool ReactionForceEnabled { get; set; } = default!;
    public Enums.ActuatorRelativeTo RelativeTo { get; set; } = default!;
    public Vector3 SecondaryTangentAxis { get; set; } = default!;
    public Vector3 VectorVelocity { get; set; } = default!;
    public Enums.VelocityConstraintMode VelocityConstraintMode { get; set; } = default!;
}
