#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIGradient", RobloxNativeType.Instance)]
public partial class UIGradient : UIComponent  {
    public ColorSequence Color { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Vector2 Offset { get; set; } = default!;
    public float Rotation { get; set; } = default!;
    public NumberSequence Transparency { get; set; } = default!;
}
