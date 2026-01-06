#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PathfindingLink", RobloxNativeType.Instance)]
public partial class PathfindingLink : Instance  {
    public Attachment Attachment0 { get; set; } = default!;
    public Attachment Attachment1 { get; set; } = default!;
    public bool IsBidirectional { get; set; } = default!;
    public string Label { get; set; } = default!;
}
