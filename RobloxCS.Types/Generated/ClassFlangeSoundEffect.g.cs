#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FlangeSoundEffect", RobloxNativeType.Instance)]
public partial class FlangeSoundEffect : SoundEffect  {
    public float Depth { get; } = default!;
    public float Mix { get; } = default!;
    public float Rate { get; } = default!;
}
