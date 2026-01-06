#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SwimController", RobloxNativeType.Instance)]
public partial class SwimController : ControllerBase  {
    public float AccelerationTime { get; set; } = default!;
    public float PitchMaxTorque { get; set; } = default!;
    public float PitchSpeedFactor { get; set; } = default!;
    public float RollMaxTorque { get; set; } = default!;
    public float RollSpeedFactor { get; set; } = default!;
}
