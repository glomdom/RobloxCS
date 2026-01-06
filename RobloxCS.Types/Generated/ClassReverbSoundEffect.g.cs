#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ReverbSoundEffect", RobloxNativeType.Instance)]
public partial class ReverbSoundEffect : SoundEffect  {
    public float DecayTime { get; set; } = default!;
    public float Density { get; set; } = default!;
    public float Diffusion { get; set; } = default!;
    public float DryLevel { get; set; } = default!;
    public float WetLevel { get; set; } = default!;
}
