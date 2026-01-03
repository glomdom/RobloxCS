#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIGridStyleLayout", RobloxNativeType.Instance)]
public partial class UIGridStyleLayout : UILayout  {
    public Vector2 AbsoluteContentSize { get; } = default!;
    public Enums.FillDirection FillDirection { get; } = default!;
    public Enums.HorizontalAlignment HorizontalAlignment { get; } = default!;
    public Enums.SortOrder SortOrder { get; } = default!;
    public Enums.VerticalAlignment VerticalAlignment { get; } = default!;
}
