#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LineForce", RobloxNativeType.Instance)]
public partial class LineForce : Constraint  {
    public bool ApplyAtCenterOfMass { get; } = default!;
    public bool InverseSquareLaw { get; } = default!;
    public float Magnitude { get; } = default!;
    public float MaxForce { get; } = default!;
    public bool ReactionForceEnabled { get; } = default!;
}
