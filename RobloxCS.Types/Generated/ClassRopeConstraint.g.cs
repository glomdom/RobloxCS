#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RopeConstraint", RobloxNativeType.Instance)]
public partial class RopeConstraint : Constraint  {
    public float CurrentDistance { get; set; } = default!;
    public float Length { get; set; } = default!;
    public float Restitution { get; set; } = default!;
    public float Thickness { get; set; } = default!;
    public bool WinchEnabled { get; set; } = default!;
    public float WinchForce { get; set; } = default!;
    public float WinchResponsiveness { get; set; } = default!;
    public float WinchSpeed { get; set; } = default!;
    public float WinchTarget { get; set; } = default!;
}
