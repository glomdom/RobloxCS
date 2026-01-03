#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BoolValue", RobloxNativeType.Instance)]
public partial class BoolValue : ValueBase  {
    public bool Value { get; } = default!;
}
