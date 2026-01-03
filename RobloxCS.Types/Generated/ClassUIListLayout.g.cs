#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIListLayout", RobloxNativeType.Instance)]
public partial class UIListLayout : UIGridStyleLayout  {
    public Enums.UIFlexAlignment HorizontalFlex { get; } = default!;
    public Enums.ItemLineAlignment ItemLineAlignment { get; } = default!;
    public UDim Padding { get; } = default!;
    public Enums.UIFlexAlignment VerticalFlex { get; } = default!;
    public bool Wraps { get; } = default!;
}
