#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TremoloSoundEffect", RobloxNativeType.Instance)]
public partial class TremoloSoundEffect : SoundEffect  {
    public float Depth { get; set; } = default!;
    public float Duty { get; set; } = default!;
    public float Frequency { get; set; } = default!;
}
