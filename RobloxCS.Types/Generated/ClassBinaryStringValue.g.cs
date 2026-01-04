#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BinaryStringValue", RobloxNativeType.Instance)]
public partial class BinaryStringValue : ValueBase  {
    new public RBXScriptSignal<string> Changed { get; private set; } = null!;
}
