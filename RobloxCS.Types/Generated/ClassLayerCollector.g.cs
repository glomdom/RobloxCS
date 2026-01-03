#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LayerCollector", RobloxNativeType.Instance)]
public partial class LayerCollector : GuiBase2d  {
    public bool Enabled { get; } = default!;
    public bool ResetOnSpawn { get; } = default!;
    public Enums.ZIndexBehavior ZIndexBehavior { get; } = default!;
}
