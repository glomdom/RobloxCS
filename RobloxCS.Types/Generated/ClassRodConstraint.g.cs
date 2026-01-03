#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RodConstraint", RobloxNativeType.Instance)]
public partial class RodConstraint : Constraint  {
    public float CurrentDistance { get; } = default!;
    public float Length { get; } = default!;
    public float LimitAngle0 { get; } = default!;
    public float LimitAngle1 { get; } = default!;
    public bool LimitsEnabled { get; } = default!;
    public float Thickness { get; } = default!;
}
