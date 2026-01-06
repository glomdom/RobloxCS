#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FlangeSoundEffect", RobloxNativeType.Instance)]
public partial class FlangeSoundEffect : SoundEffect  {
    public float Depth { get; set; } = default!;
    public float Mix { get; set; } = default!;
    public float Rate { get; set; } = default!;
}
