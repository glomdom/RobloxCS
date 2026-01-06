#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AdGui", RobloxNativeType.Instance)]
public partial class AdGui : SurfaceGuiBase  {
    public Enums.AdShape AdShape { get; set; } = default!;
    public bool EnableVideoAds { get; set; } = default!;
    public string FallbackImage { get; set; } = default!;
    public Enums.AdUnitStatus Status { get; set; } = default!;
    public Func<Dictionary<string, object>> OnAdEvent { get; set; } = default!;
}
