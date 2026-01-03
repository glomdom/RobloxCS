#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AirController", RobloxNativeType.Instance)]
public partial class AirController : ControllerBase  {
    public float BalanceMaxTorque { get; } = default!;
    public float BalanceSpeed { get; } = default!;
    public bool MaintainAngularMomentum { get; } = default!;
    public bool MaintainLinearMomentum { get; } = default!;
    public float MoveMaxForce { get; } = default!;
    public float TurnMaxTorque { get; } = default!;
    public float TurnSpeedFactor { get; } = default!;
}
