#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerGui", RobloxNativeType.Instance)]
public partial class PlayerGui : BasePlayerGui  {
    public Enums.ScreenOrientation CurrentScreenOrientation { get; set; } = default!;
    public Enums.ScreenOrientation ScreenOrientation { get; set; } = default!;
    public GuiObject SelectionImageObject { get; set; } = default!;
}
