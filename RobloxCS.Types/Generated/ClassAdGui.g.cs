#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AdGui", RobloxNativeType.Instance)]
public partial class AdGui : SurfaceGuiBase  {
    public Enums.AdShape AdShape { get; } = default!;
    public bool EnableVideoAds { get; } = default!;
    public string FallbackImage { get; } = default!;
    public Enums.AdUnitStatus Status { get; } = default!;
}
