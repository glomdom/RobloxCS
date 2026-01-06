#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AnimationConstraint", RobloxNativeType.Instance)]
public partial class AnimationConstraint : Constraint  {
    public bool IsKinematic { get; set; } = default!;
    public float MaxForce { get; set; } = default!;
    public float MaxTorque { get; set; } = default!;
    public CFrame Transform { get; set; } = default!;
}
