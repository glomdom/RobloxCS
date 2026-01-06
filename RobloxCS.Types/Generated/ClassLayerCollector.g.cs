#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LayerCollector", RobloxNativeType.Instance)]
public partial class LayerCollector : GuiBase2d  {
    public bool Enabled { get; set; } = default!;
    public bool ResetOnSpawn { get; set; } = default!;
    public Enums.ZIndexBehavior ZIndexBehavior { get; set; } = default!;
}
