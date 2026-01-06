#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WrapTarget", RobloxNativeType.Instance)]
public partial class WrapTarget : BaseWrap  {
    public Color3 Color { get; set; } = default!;
    public Enums.WrapTargetDebugMode DebugMode { get; set; } = default!;
    public float Stiffness { get; } = default!;
}
