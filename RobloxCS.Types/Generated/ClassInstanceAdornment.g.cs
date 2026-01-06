#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InstanceAdornment", RobloxNativeType.Instance)]
public partial class InstanceAdornment : GuiBase3d  {
    public Instance Adornee { get; set; } = default!;
}
