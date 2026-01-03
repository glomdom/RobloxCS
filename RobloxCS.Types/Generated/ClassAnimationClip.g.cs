#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AnimationClip", RobloxNativeType.Instance)]
public partial class AnimationClip : Instance  {
    public bool Loop { get; } = default!;
    public Enums.AnimationPriority Priority { get; } = default!;
}
