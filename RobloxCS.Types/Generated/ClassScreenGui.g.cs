#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScreenGui", RobloxNativeType.Instance)]
public partial class ScreenGui : LayerCollector  {
    public bool ClipToDeviceSafeArea { get; } = default!;
    public int DisplayOrder { get; } = default!;
    public bool IgnoreGuiInset { get; } = default!;
    public Enums.SafeAreaCompatibility SafeAreaCompatibility { get; } = default!;
    public Enums.ScreenInsets ScreenInsets { get; } = default!;
}
