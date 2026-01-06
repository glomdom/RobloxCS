#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CanvasGroup", RobloxNativeType.Instance)]
public partial class CanvasGroup : GuiObject  {
    public Color3 GroupColor3 { get; set; } = default!;
    public float GroupTransparency { get; set; } = default!;
}
