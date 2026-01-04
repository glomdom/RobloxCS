#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StringValue", RobloxNativeType.Instance)]
public partial class StringValue : ValueBase  {
    public string Value { get; } = default!;
    new public RBXScriptSignal<string> Changed { get; private set; } = null!;
}
