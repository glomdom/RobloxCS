#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIGridStyleLayout", RobloxNativeType.Instance)]
public partial class UIGridStyleLayout : UILayout  {
    public Vector2 AbsoluteContentSize { get; set; } = default!;
    public Enums.FillDirection FillDirection { get; set; } = default!;
    public Enums.HorizontalAlignment HorizontalAlignment { get; set; } = default!;
    public Enums.SortOrder SortOrder { get; set; } = default!;
    public Enums.VerticalAlignment VerticalAlignment { get; set; } = default!;
}
