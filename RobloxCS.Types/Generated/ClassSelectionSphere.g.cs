#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SelectionSphere", RobloxNativeType.Instance)]
public partial class SelectionSphere : PVAdornment  {
    public Color3 SurfaceColor3 { get; set; } = default!;
    public float SurfaceTransparency { get; set; } = default!;
}
