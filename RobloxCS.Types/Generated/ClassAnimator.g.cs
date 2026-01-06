#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Animator", RobloxNativeType.Instance)]
public partial class Animator : Instance  {
    public bool EvaluationThrottled { get; set; } = default!;
    public bool PreferLodEnabled { get; set; } = default!;
    public CFrame RootMotion { get; set; } = default!;
    public float RootMotionWeight { get; set; } = default!;
    public void ApplyJointVelocities() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetPlayingAnimationTracks() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public AnimationTrack LoadAnimation() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void RegisterEvaluationParallelCallback() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<AnimationTrack> AnimationPlayed { get; private set; } = null!;
}
