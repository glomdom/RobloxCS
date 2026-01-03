#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIFlexItem", RobloxNativeType.Instance)]
public partial class UIFlexItem : UIComponent  {
    public Enums.UIFlexMode FlexMode { get; } = default!;
    public float GrowRatio { get; } = default!;
    public Enums.ItemLineAlignment ItemLineAlignment { get; } = default!;
    public float ShrinkRatio { get; } = default!;
}
