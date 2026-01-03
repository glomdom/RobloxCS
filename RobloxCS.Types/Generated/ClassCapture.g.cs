#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Capture", RobloxNativeType.Instance)]
public partial class Capture : Object  {
    public DateTime CaptureTime { get; } = default!;
    public Enums.CaptureType CaptureType { get; } = default!;
    public string LocalId { get; } = default!;
    public int SourcePlaceId { get; } = default!;
    public int SourceUniverseId { get; } = default!;
}
