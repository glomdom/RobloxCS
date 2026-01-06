#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AngularVelocity", RobloxNativeType.Instance)]
public partial class AngularVelocity : Constraint  {
    [RobloxName("AngularVelocity")]
    public Vector3 Value { get; set; } = default!;
    public float MaxTorque { get; set; } = default!;
    public bool ReactionTorqueEnabled { get; set; } = default!;
    public Enums.ActuatorRelativeTo RelativeTo { get; set; } = default!;
}
