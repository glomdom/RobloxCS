#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VectorForce", RobloxNativeType.Instance)]
public partial class VectorForce : Constraint  {
    public bool ApplyAtCenterOfMass { get; } = default!;
    public Vector3 Force { get; } = default!;
    public Enums.ActuatorRelativeTo RelativeTo { get; } = default!;
}
