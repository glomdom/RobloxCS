#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NumberValue", RobloxNativeType.Instance)]
public partial class NumberValue : ValueBase  {
    public double Value { get; set; } = default!;
    new public RBXScriptSignal<double> Changed { get; private set; } = null!;
}
