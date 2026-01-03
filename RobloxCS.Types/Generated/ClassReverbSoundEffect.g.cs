#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ReverbSoundEffect", RobloxNativeType.Instance)]
public partial class ReverbSoundEffect : SoundEffect  {
    public float DecayTime { get; } = default!;
    public float Density { get; } = default!;
    public float Diffusion { get; } = default!;
    public float DryLevel { get; } = default!;
    public float WetLevel { get; } = default!;
}
