#nullable enable
namespace RobloxCS.Types;
[RobloxNative("EqualizerSoundEffect", RobloxNativeType.Instance)]
public partial class EqualizerSoundEffect : SoundEffect  {
    public float HighGain { get; set; } = default!;
    public float LowGain { get; set; } = default!;
    public float MidGain { get; set; } = default!;
}
