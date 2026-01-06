#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PitchShiftSoundEffect", RobloxNativeType.Instance)]
public partial class PitchShiftSoundEffect : SoundEffect  {
    public float Octave { get; set; } = default!;
}
