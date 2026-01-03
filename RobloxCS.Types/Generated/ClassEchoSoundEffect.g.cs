#nullable enable
namespace RobloxCS.Types;
[RobloxNative("EchoSoundEffect", RobloxNativeType.Instance)]
public partial class EchoSoundEffect : SoundEffect  {
    public float Delay { get; } = default!;
    public float DryLevel { get; } = default!;
    public float Feedback { get; } = default!;
    public float WetLevel { get; } = default!;
}
