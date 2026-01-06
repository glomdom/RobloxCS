#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SpringConstraint", RobloxNativeType.Instance)]
public partial class SpringConstraint : Constraint  {
    public float Coils { get; set; } = default!;
    public float CurrentLength { get; set; } = default!;
    public float Damping { get; set; } = default!;
    public float FreeLength { get; set; } = default!;
    public bool LimitsEnabled { get; set; } = default!;
    public float MaxForce { get; set; } = default!;
    public float MaxLength { get; set; } = default!;
    public float MinLength { get; set; } = default!;
    public float Radius { get; set; } = default!;
    public float Stiffness { get; set; } = default!;
    public float Thickness { get; set; } = default!;
}
