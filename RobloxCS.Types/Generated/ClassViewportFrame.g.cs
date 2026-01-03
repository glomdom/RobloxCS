#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ViewportFrame", RobloxNativeType.Instance)]
public partial class ViewportFrame : GuiObject  {
    public Color3 Ambient { get; } = default!;
    public Camera CurrentCamera { get; } = default!;
    public Color3 ImageColor3 { get; } = default!;
    public float ImageTransparency { get; } = default!;
    public Color3 LightColor { get; } = default!;
    public Vector3 LightDirection { get; } = default!;
}
