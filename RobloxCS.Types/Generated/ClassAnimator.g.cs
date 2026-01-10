#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Animator", RobloxNativeType.Instance)]
public partial class Animator : Instance  {
    public bool EvaluationThrottled { get; set; } = default!;
    public bool PreferLodEnabled { get; set; } = default!;
    public CFrame RootMotion { get; set; } = default!;
    public float RootMotionWeight { get; set; } = default!;
    public void ApplyJointVelocities() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetPlayingAnimationTracks() => ThrowHelper.ThrowTranspiledMethod();
    public AnimationTrack LoadAnimation() => ThrowHelper.ThrowTranspiledMethod();
    public void RegisterEvaluationParallelCallback() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<AnimationTrack> AnimationPlayed { get; private set; } = null!;
}
