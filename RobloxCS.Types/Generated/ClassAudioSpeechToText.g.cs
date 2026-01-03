#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AudioSpeechToText", RobloxNativeType.Instance)]
public partial class AudioSpeechToText : Instance  {
    public bool Enabled { get; } = default!;
    public string Text { get; } = default!;
    public bool VoiceDetected { get; } = default!;
    public List<Instance> GetConnectedWires() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
