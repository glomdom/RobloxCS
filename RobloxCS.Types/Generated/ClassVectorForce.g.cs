#nullable enable
namespace RobloxCS.Types;
[RobloxNative("VectorForce", RobloxNativeType.Instance)]
public partial class VectorForce : Constraint  {
    public bool ApplyAtCenterOfMass { get; set; } = default!;
    public Vector3 Force { get; set; } = default!;
    public Enums.ActuatorRelativeTo RelativeTo { get; set; } = default!;
}
