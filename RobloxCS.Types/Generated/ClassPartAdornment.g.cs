#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PartAdornment", RobloxNativeType.Instance)]
public partial class PartAdornment : GuiBase3d  {
    public BasePart Adornee { get; } = default!;
}
