#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Frame", RobloxNativeType.Instance)]
public partial class Frame : GuiObject  {
    public Enums.FrameStyle Style { get; set; } = default!;
}
