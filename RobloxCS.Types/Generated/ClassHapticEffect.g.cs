#nullable enable
namespace RobloxCS.Types;
[RobloxNative("HapticEffect", RobloxNativeType.Instance)]
public partial class HapticEffect : Instance  {
    public bool Looped { get; } = default!;
    public Vector3 Position { get; } = default!;
    public float Radius { get; } = default!;
    public Enums.HapticEffectType Type { get; } = default!;
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetWaveformKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Stop() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
