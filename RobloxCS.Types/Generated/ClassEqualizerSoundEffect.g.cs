#nullable enable
namespace RobloxCS.Types;
[RobloxNative("EqualizerSoundEffect", RobloxNativeType.Instance)]
public partial class EqualizerSoundEffect : SoundEffect  {
    public float HighGain { get; } = default!;
    public float LowGain { get; } = default!;
    public float MidGain { get; } = default!;
}
