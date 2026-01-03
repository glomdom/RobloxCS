#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ControllerBase", RobloxNativeType.Instance)]
public partial class ControllerBase : Instance  {
    public bool Active { get; } = default!;
    public bool BalanceRigidityEnabled { get; } = default!;
    public float MoveSpeedFactor { get; } = default!;
}
