#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIPadding", RobloxNativeType.Instance)]
public partial class UIPadding : UIComponent  {
    public UDim PaddingBottom { get; } = default!;
    public UDim PaddingLeft { get; } = default!;
    public UDim PaddingRight { get; } = default!;
    public UDim PaddingTop { get; } = default!;
}
