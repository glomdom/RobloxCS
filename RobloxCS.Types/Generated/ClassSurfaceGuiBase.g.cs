#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceGuiBase", RobloxNativeType.Instance)]
public partial class SurfaceGuiBase : LayerCollector  {
    public bool Active { get; } = default!;
    public Instance Adornee { get; } = default!;
    public Enums.NormalId Face { get; } = default!;
}
