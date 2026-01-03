#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SwimController", RobloxNativeType.Instance)]
public partial class SwimController : ControllerBase  {
    public float AccelerationTime { get; } = default!;
    public float PitchMaxTorque { get; } = default!;
    public float PitchSpeedFactor { get; } = default!;
    public float RollMaxTorque { get; } = default!;
    public float RollSpeedFactor { get; } = default!;
}
