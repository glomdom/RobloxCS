#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UITextSizeConstraint", RobloxNativeType.Instance)]
public partial class UITextSizeConstraint : UIConstraint  {
    public int MaxTextSize { get; set; } = default!;
    public int MinTextSize { get; set; } = default!;
}
