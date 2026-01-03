#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AlignPosition", RobloxNativeType.Instance)]
public partial class AlignPosition : Constraint  {
    public bool ApplyAtCenterOfMass { get; } = default!;
    public Enums.ForceLimitMode ForceLimitMode { get; } = default!;
    public Enums.ActuatorRelativeTo ForceRelativeTo { get; } = default!;
    public Vector3 MaxAxesForce { get; } = default!;
    public float MaxForce { get; } = default!;
    public float MaxVelocity { get; } = default!;
    public Enums.PositionAlignmentMode Mode { get; } = default!;
    public Vector3 Position { get; } = default!;
    public bool ReactionForceEnabled { get; } = default!;
    public float Responsiveness { get; } = default!;
    public bool RigidityEnabled { get; } = default!;
}
