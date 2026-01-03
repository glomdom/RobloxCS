#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UniversalConstraint", RobloxNativeType.Instance)]
public partial class UniversalConstraint : Constraint  {
    public bool LimitsEnabled { get; } = default!;
    public float MaxAngle { get; } = default!;
    public float Radius { get; } = default!;
    public float Restitution { get; } = default!;
}
