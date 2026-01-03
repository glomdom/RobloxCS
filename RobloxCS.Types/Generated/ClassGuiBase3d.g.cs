#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiBase3d", RobloxNativeType.Instance)]
public partial class GuiBase3d : GuiBase  {
    public Color3 Color3 { get; } = default!;
    public float Transparency { get; } = default!;
    public bool Visible { get; } = default!;
}
