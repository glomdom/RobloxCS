#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AlignPosition", RobloxNativeType.Instance)]
public partial class AlignPosition : Constraint  {
    public bool ApplyAtCenterOfMass { get; set; } = default!;
    public Enums.ForceLimitMode ForceLimitMode { get; set; } = default!;
    public Enums.ActuatorRelativeTo ForceRelativeTo { get; set; } = default!;
    public Vector3 MaxAxesForce { get; set; } = default!;
    public float MaxForce { get; set; } = default!;
    public float MaxVelocity { get; set; } = default!;
    public Enums.PositionAlignmentMode Mode { get; set; } = default!;
    public Vector3 Position { get; set; } = default!;
    public bool ReactionForceEnabled { get; set; } = default!;
    public float Responsiveness { get; set; } = default!;
    public bool RigidityEnabled { get; set; } = default!;
}
