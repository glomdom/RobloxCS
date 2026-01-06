#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Color3Value", RobloxNativeType.Instance)]
public partial class Color3Value : ValueBase  {
    public Color3 Value { get; set; } = default!;
    new public RBXScriptSignal<Color3> Changed { get; private set; } = null!;
}
