#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIFlexItem", RobloxNativeType.Instance)]
public partial class UIFlexItem : UIComponent  {
    public Enums.UIFlexMode FlexMode { get; set; } = default!;
    public float GrowRatio { get; set; } = default!;
    public Enums.ItemLineAlignment ItemLineAlignment { get; set; } = default!;
    public float ShrinkRatio { get; set; } = default!;
}
