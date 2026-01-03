#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TremoloSoundEffect", RobloxNativeType.Instance)]
public partial class TremoloSoundEffect : SoundEffect  {
    public float Depth { get; } = default!;
    public float Duty { get; } = default!;
    public float Frequency { get; } = default!;
}
