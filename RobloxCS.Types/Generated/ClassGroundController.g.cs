#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GroundController", RobloxNativeType.Instance)]
public partial class GroundController : ControllerBase  {
    public float AccelerationLean { get; set; } = default!;
    public float AccelerationTime { get; set; } = default!;
    public float BalanceMaxTorque { get; set; } = default!;
    public float BalanceSpeed { get; set; } = default!;
    public float DecelerationTime { get; set; } = default!;
    public float Friction { get; set; } = default!;
    public float FrictionWeight { get; set; } = default!;
    public float GroundOffset { get; set; } = default!;
    public float StandForce { get; set; } = default!;
    public float StandSpeed { get; set; } = default!;
    public float TurnSpeedFactor { get; set; } = default!;
}
