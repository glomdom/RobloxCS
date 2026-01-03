#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CompressorSoundEffect", RobloxNativeType.Instance)]
public partial class CompressorSoundEffect : SoundEffect  {
    public float Attack { get; } = default!;
    public float GainMakeup { get; } = default!;
    public float Ratio { get; } = default!;
    public float Release { get; } = default!;
    public Instance SideChain { get; } = default!;
    public float Threshold { get; } = default!;
}
