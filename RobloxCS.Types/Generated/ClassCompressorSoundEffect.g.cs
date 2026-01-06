#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CompressorSoundEffect", RobloxNativeType.Instance)]
public partial class CompressorSoundEffect : SoundEffect  {
    public float Attack { get; set; } = default!;
    public float GainMakeup { get; set; } = default!;
    public float Ratio { get; set; } = default!;
    public float Release { get; set; } = default!;
    public Instance SideChain { get; set; } = default!;
    public float Threshold { get; set; } = default!;
}
