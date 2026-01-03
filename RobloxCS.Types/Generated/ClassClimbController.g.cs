#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ClimbController", RobloxNativeType.Instance)]
public partial class ClimbController : ControllerBase  {
    public float AccelerationTime { get; } = default!;
    public float BalanceMaxTorque { get; } = default!;
    public float BalanceSpeed { get; } = default!;
    public float MoveMaxForce { get; } = default!;
}
