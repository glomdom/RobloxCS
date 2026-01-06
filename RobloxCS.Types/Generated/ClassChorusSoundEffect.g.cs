#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChorusSoundEffect", RobloxNativeType.Instance)]
public partial class ChorusSoundEffect : SoundEffect  {
    public float Depth { get; set; } = default!;
    public float Mix { get; set; } = default!;
    public float Rate { get; set; } = default!;
}
