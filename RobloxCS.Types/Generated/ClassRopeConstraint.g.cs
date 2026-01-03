#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RopeConstraint", RobloxNativeType.Instance)]
public partial class RopeConstraint : Constraint  {
    public float CurrentDistance { get; } = default!;
    public float Length { get; } = default!;
    public float Restitution { get; } = default!;
    public float Thickness { get; } = default!;
    public bool WinchEnabled { get; } = default!;
    public float WinchForce { get; } = default!;
    public float WinchResponsiveness { get; } = default!;
    public float WinchSpeed { get; } = default!;
    public float WinchTarget { get; } = default!;
}
