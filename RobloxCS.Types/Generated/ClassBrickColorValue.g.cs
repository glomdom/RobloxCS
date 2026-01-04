#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BrickColorValue", RobloxNativeType.Instance)]
public partial class BrickColorValue : ValueBase  {
    public BrickColor Value { get; } = default!;
    new public RBXScriptSignal<BrickColor> Changed { get; private set; } = null!;
}
