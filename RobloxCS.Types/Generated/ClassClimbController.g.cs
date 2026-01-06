#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ClimbController", RobloxNativeType.Instance)]
public partial class ClimbController : ControllerBase  {
    public float AccelerationTime { get; set; } = default!;
    public float BalanceMaxTorque { get; set; } = default!;
    public float BalanceSpeed { get; set; } = default!;
    public float MoveMaxForce { get; set; } = default!;
}
