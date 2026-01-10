#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HapticEffect", RobloxNativeType.Instance)]
public partial class HapticEffect : Instance  {
    public bool Looped { get; set; } = default!;
    public Vector3 Position { get; set; } = default!;
    public float Radius { get; set; } = default!;
    public Enums.HapticEffectType Type { get; set; } = default!;
    public void Play() => ThrowHelper.ThrowTranspiledMethod();
    public void SetWaveformKeys() => ThrowHelper.ThrowTranspiledMethod();
    public void Stop() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal Ended { get; private set; } = null!;
}
