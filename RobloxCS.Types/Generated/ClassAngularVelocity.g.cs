#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AngularVelocity", RobloxNativeType.Instance)]
public partial class AngularVelocity : Constraint  {
    [RobloxName("AngularVelocity")]
    public Vector3 Value { get; } = default!;
    public float MaxTorque { get; } = default!;
    public bool ReactionTorqueEnabled { get; } = default!;
    public Enums.ActuatorRelativeTo RelativeTo { get; } = default!;
}
