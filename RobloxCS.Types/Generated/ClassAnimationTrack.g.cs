#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AnimationTrack", RobloxNativeType.Instance)]
public partial class AnimationTrack : Instance  {
    public Animation Animation { get; } = default!;
    public bool IsPlaying { get; } = default!;
    public float Length { get; } = default!;
    public bool Looped { get; } = default!;
    public Enums.AnimationPriority Priority { get; } = default!;
    public float Speed { get; } = default!;
    public float TimePosition { get; } = default!;
    public float WeightCurrent { get; } = default!;
    public float WeightTarget { get; } = default!;
    public void AdjustSpeed() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void AdjustWeight() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal GetMarkerReachedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetParameter() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Dictionary<string, object> GetParameterDefaults() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance GetTargetInstance() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetTargetNames() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public double GetTimeOfKeyframe() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetParameter() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetTargetInstance() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Stop() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal DidLoop { get; private set; } = null!;
    public RBXScriptSignal Ended { get; private set; } = null!;
    public RBXScriptSignal<string> KeyframeReached { get; private set; } = null!;
    public RBXScriptSignal Stopped { get; private set; } = null!;
}
