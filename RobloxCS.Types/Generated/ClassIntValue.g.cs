#nullable enable
namespace RobloxCS.Types;
[RobloxNative("IntValue", RobloxNativeType.Instance)]
public partial class IntValue : ValueBase  {
    public int Value { get; } = default!;
}
