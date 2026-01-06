#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TrackerLodController", RobloxNativeType.Instance)]
public partial class TrackerLodController : Instance  {
    public Enums.TrackerLodFlagMode AudioMode { get; set; } = default!;
    public Enums.TrackerExtrapolationFlagMode VideoExtrapolationMode { get; set; } = default!;
    public Enums.TrackerLodValueMode VideoLodMode { get; set; } = default!;
    public Enums.TrackerLodFlagMode VideoMode { get; set; } = default!;
}
