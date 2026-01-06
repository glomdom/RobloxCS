#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIPadding", RobloxNativeType.Instance)]
public partial class UIPadding : UIComponent  {
    public UDim PaddingBottom { get; set; } = default!;
    public UDim PaddingLeft { get; set; } = default!;
    public UDim PaddingRight { get; set; } = default!;
    public UDim PaddingTop { get; set; } = default!;
}
