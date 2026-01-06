#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UniversalConstraint", RobloxNativeType.Instance)]
public partial class UniversalConstraint : Constraint  {
    public bool LimitsEnabled { get; set; } = default!;
    public float MaxAngle { get; set; } = default!;
    public float Radius { get; set; } = default!;
    public float Restitution { get; set; } = default!;
}
