#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleDerive", RobloxNativeType.Instance)]
public partial class StyleDerive : Instance  {
    public int Priority { get; } = default!;
    public StyleSheet StyleSheet { get; } = default!;
}
