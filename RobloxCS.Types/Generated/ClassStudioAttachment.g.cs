#nullable enable
namespace RobloxCS.Types;
[RobloxNative("StudioAttachment", RobloxNativeType.Instance)]
public partial class StudioAttachment : Instance  {
    public bool AutoHideParent { get; set; } = default!;
    public bool IsArrowVisible { get; set; } = default!;
    public Vector2 Offset { get; set; } = default!;
    public Vector2 SourceAnchorPoint { get; set; } = default!;
    public Vector2 TargetAnchorPoint { get; set; } = default!;
}
