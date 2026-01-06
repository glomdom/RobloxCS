#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LineForce", RobloxNativeType.Instance)]
public partial class LineForce : Constraint  {
    public bool ApplyAtCenterOfMass { get; set; } = default!;
    public bool InverseSquareLaw { get; set; } = default!;
    public float Magnitude { get; set; } = default!;
    public float MaxForce { get; set; } = default!;
    public bool ReactionForceEnabled { get; set; } = default!;
}
