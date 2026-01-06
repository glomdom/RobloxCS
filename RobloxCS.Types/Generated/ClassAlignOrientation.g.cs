#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AlignOrientation", RobloxNativeType.Instance)]
public partial class AlignOrientation : Constraint  {
    public Enums.AlignType AlignType { get; set; } = default!;
    public CFrame CFrame { get; set; } = default!;
    public Vector3 LookAtPosition { get; set; } = default!;
    public float MaxAngularVelocity { get; set; } = default!;
    public float MaxTorque { get; set; } = default!;
    public Enums.OrientationAlignmentMode Mode { get; set; } = default!;
    public Vector3 PrimaryAxis { get; set; } = default!;
    public bool PrimaryAxisOnly { get; set; } = default!;
    public bool ReactionTorqueEnabled { get; set; } = default!;
    public float Responsiveness { get; set; } = default!;
    public bool RigidityEnabled { get; set; } = default!;
    public Vector3 SecondaryAxis { get; set; } = default!;
}
