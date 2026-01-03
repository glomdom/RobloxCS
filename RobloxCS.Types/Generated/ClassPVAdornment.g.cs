#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PVAdornment", RobloxNativeType.Instance)]
public partial class PVAdornment : GuiBase3d  {
    public PVInstance Adornee { get; } = default!;
}
