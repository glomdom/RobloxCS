#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CanvasGroup", RobloxNativeType.Instance)]
public partial class CanvasGroup : GuiObject  {
    public Color3 GroupColor3 { get; } = default!;
    public float GroupTransparency { get; } = default!;
}
