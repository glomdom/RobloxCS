#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UITextSizeConstraint", RobloxNativeType.Instance)]
public partial class UITextSizeConstraint : UIConstraint  {
    public int MaxTextSize { get; } = default!;
    public int MinTextSize { get; } = default!;
}
