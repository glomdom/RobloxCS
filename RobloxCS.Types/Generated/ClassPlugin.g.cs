#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Plugin", RobloxNativeType.Instance)]
public partial class Plugin : Instance  {
    public bool CollisionEnabled { get; set; } = default!;
    public float GridSize { get; set; } = default!;
}
