#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Plugin", RobloxNativeType.Instance)]
public partial class Plugin : Instance  {
    public bool CollisionEnabled { get; } = default!;
    public float GridSize { get; } = default!;
}
