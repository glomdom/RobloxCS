#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceSelection", RobloxNativeType.Instance)]
public partial class SurfaceSelection : PartAdornment  {
    public Enums.NormalId TargetSurface { get; } = default!;
}
