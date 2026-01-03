#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AlignOrientation", RobloxNativeType.Instance)]
public partial class AlignOrientation : Constraint  {
    public Enums.AlignType AlignType { get; } = default!;
    public CFrame CFrame { get; } = default!;
    public Vector3 LookAtPosition { get; } = default!;
    public float MaxAngularVelocity { get; } = default!;
    public float MaxTorque { get; } = default!;
    public Enums.OrientationAlignmentMode Mode { get; } = default!;
    public Vector3 PrimaryAxis { get; } = default!;
    public bool PrimaryAxisOnly { get; } = default!;
    public bool ReactionTorqueEnabled { get; } = default!;
    public float Responsiveness { get; } = default!;
    public bool RigidityEnabled { get; } = default!;
    public Vector3 SecondaryAxis { get; } = default!;
}
