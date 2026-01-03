#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GroundController", RobloxNativeType.Instance)]
public partial class GroundController : ControllerBase  {
    public float AccelerationLean { get; } = default!;
    public float AccelerationTime { get; } = default!;
    public float BalanceMaxTorque { get; } = default!;
    public float BalanceSpeed { get; } = default!;
    public float DecelerationTime { get; } = default!;
    public float Friction { get; } = default!;
    public float FrictionWeight { get; } = default!;
    public float GroundOffset { get; } = default!;
    public float StandForce { get; } = default!;
    public float StandSpeed { get; } = default!;
    public float TurnSpeedFactor { get; } = default!;
}
