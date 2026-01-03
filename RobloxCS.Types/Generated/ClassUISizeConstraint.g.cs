#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UISizeConstraint", RobloxNativeType.Instance)]
public partial class UISizeConstraint : UIConstraint  {
    public Vector2 MaxSize { get; } = default!;
    public Vector2 MinSize { get; } = default!;
}
