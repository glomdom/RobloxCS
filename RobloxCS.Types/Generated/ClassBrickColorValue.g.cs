#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BrickColorValue", RobloxNativeType.Instance)]
public partial class BrickColorValue : ValueBase  {
    public BrickColor Value { get; set; } = default!;
    new public RBXScriptSignal<BrickColor> Changed { get; private set; } = null!;
}
