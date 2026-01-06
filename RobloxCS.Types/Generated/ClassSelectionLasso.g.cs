#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SelectionLasso", RobloxNativeType.Instance)]
public partial class SelectionLasso : GuiBase3d  {
    public Humanoid Humanoid { get; set; } = default!;
}
