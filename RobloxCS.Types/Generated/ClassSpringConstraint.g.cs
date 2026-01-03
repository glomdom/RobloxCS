#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SpringConstraint", RobloxNativeType.Instance)]
public partial class SpringConstraint : Constraint  {
    public float Coils { get; } = default!;
    public float CurrentLength { get; } = default!;
    public float Damping { get; } = default!;
    public float FreeLength { get; } = default!;
    public bool LimitsEnabled { get; } = default!;
    public float MaxForce { get; } = default!;
    public float MaxLength { get; } = default!;
    public float MinLength { get; } = default!;
    public float Radius { get; } = default!;
    public float Stiffness { get; } = default!;
    public float Thickness { get; } = default!;
}
