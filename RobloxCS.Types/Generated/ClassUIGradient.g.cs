#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIGradient", RobloxNativeType.Instance)]
public partial class UIGradient : UIComponent  {
    public ColorSequence Color { get; } = default!;
    public bool Enabled { get; } = default!;
    public Vector2 Offset { get; } = default!;
    public float Rotation { get; } = default!;
    public NumberSequence Transparency { get; } = default!;
}
