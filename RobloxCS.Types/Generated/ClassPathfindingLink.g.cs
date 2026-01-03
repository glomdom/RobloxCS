#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PathfindingLink", RobloxNativeType.Instance)]
public partial class PathfindingLink : Instance  {
    public Attachment Attachment0 { get; } = default!;
    public Attachment Attachment1 { get; } = default!;
    public bool IsBidirectional { get; } = default!;
    public string Label { get; } = default!;
}
