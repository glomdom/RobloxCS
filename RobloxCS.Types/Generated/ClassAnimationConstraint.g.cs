#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AnimationConstraint", RobloxNativeType.Instance)]
public partial class AnimationConstraint : Constraint  {
    public bool IsKinematic { get; } = default!;
    public float MaxForce { get; } = default!;
    public float MaxTorque { get; } = default!;
    public CFrame Transform { get; } = default!;
}
