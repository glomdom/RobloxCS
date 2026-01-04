#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ObjectValue", RobloxNativeType.Instance)]
public partial class ObjectValue : ValueBase  {
    public Instance Value { get; } = default!;
    new public RBXScriptSignal<Instance> Changed { get; private set; } = null!;
}
