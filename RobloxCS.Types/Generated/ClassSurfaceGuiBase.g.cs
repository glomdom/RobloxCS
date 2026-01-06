#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceGuiBase", RobloxNativeType.Instance)]
public partial class SurfaceGuiBase : LayerCollector  {
    public bool Active { get; set; } = default!;
    public Instance Adornee { get; set; } = default!;
    public Enums.NormalId Face { get; set; } = default!;
}
