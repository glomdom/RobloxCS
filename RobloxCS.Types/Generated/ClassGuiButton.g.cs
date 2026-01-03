#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiButton", RobloxNativeType.Instance)]
public partial class GuiButton : GuiObject  {
    public bool AutoButtonColor { get; } = default!;
    public HapticEffect HoverHapticEffect { get; } = default!;
    public bool Modal { get; } = default!;
    public HapticEffect PressHapticEffect { get; } = default!;
    public bool Selected { get; } = default!;
    public Enums.ButtonStyle Style { get; } = default!;
}
