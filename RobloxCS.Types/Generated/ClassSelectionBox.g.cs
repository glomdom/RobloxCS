#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SelectionBox", RobloxNativeType.Instance)]
public partial class SelectionBox : InstanceAdornment  {
    public float LineThickness { get; set; } = default!;
    public Color3 SurfaceColor3 { get; set; } = default!;
    public float SurfaceTransparency { get; set; } = default!;
}
