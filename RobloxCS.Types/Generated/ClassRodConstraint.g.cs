#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RodConstraint", RobloxNativeType.Instance)]
public partial class RodConstraint : Constraint  {
    public float CurrentDistance { get; set; } = default!;
    public float Length { get; set; } = default!;
    public float LimitAngle0 { get; set; } = default!;
    public float LimitAngle1 { get; set; } = default!;
    public bool LimitsEnabled { get; set; } = default!;
    public float Thickness { get; set; } = default!;
}
