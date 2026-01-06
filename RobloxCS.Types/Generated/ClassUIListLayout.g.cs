#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIListLayout", RobloxNativeType.Instance)]
public partial class UIListLayout : UIGridStyleLayout  {
    public Enums.UIFlexAlignment HorizontalFlex { get; set; } = default!;
    public Enums.ItemLineAlignment ItemLineAlignment { get; set; } = default!;
    public UDim Padding { get; set; } = default!;
    public Enums.UIFlexAlignment VerticalFlex { get; set; } = default!;
    public bool Wraps { get; set; } = default!;
}
