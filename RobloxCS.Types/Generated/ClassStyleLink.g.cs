#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StyleLink", RobloxNativeType.Instance)]
public partial class StyleLink : Instance  {
    public StyleSheet StyleSheet { get; } = default!;
}
