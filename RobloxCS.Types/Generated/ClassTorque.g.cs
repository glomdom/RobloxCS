#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Torque", RobloxNativeType.Instance)]
public partial class Torque : Constraint  {
    public Enums.ActuatorRelativeTo RelativeTo { get; set; } = default!;
    [RobloxName("Torque")]
    public Vector3 Value { get; set; } = default!;
}
