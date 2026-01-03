#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SelectionBox", RobloxNativeType.Instance)]
public partial class SelectionBox : InstanceAdornment  {
    public float LineThickness { get; } = default!;
    public Color3 SurfaceColor3 { get; } = default!;
    public float SurfaceTransparency { get; } = default!;
}
