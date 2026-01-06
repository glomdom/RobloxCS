#nullable enable
namespace RobloxCS.Types;
[RobloxNative("EchoSoundEffect", RobloxNativeType.Instance)]
public partial class EchoSoundEffect : SoundEffect  {
    public float Delay { get; set; } = default!;
    public float DryLevel { get; set; } = default!;
    public float Feedback { get; set; } = default!;
    public float WetLevel { get; set; } = default!;
}
