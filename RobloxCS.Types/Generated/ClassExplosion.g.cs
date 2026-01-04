#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Explosion", RobloxNativeType.Instance)]
public partial class Explosion : Instance  {
    public float BlastPressure { get; } = default!;
    public float BlastRadius { get; } = default!;
    public float DestroyJointRadiusPercent { get; } = default!;
    public Enums.ExplosionType ExplosionType { get; } = default!;
    public Vector3 Position { get; } = default!;
    public float TimeScale { get; } = default!;
    public bool Visible { get; } = default!;
    public RBXScriptSignal<BasePart, float> Hit { get; private set; } = null!;
}
