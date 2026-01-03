#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StudioAttachment", RobloxNativeType.Instance)]
public partial class StudioAttachment : Instance  {
    public bool AutoHideParent { get; } = default!;
    public bool IsArrowVisible { get; } = default!;
    public Vector2 Offset { get; } = default!;
    public Vector2 SourceAnchorPoint { get; } = default!;
    public Vector2 TargetAnchorPoint { get; } = default!;
}
