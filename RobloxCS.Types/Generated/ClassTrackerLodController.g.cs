#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TrackerLodController", RobloxNativeType.Instance)]
public partial class TrackerLodController : Instance  {
    public Enums.TrackerLodFlagMode AudioMode { get; } = default!;
    public Enums.TrackerExtrapolationFlagMode VideoExtrapolationMode { get; } = default!;
    public Enums.TrackerLodValueMode VideoLodMode { get; } = default!;
    public Enums.TrackerLodFlagMode VideoMode { get; } = default!;
}
