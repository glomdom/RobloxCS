#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PlayerGui", RobloxNativeType.Instance)]
public partial class PlayerGui : BasePlayerGui  {
    public Enums.ScreenOrientation CurrentScreenOrientation { get; } = default!;
    public Enums.ScreenOrientation ScreenOrientation { get; } = default!;
    public GuiObject SelectionImageObject { get; } = default!;
}
