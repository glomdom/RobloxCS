#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AnimationClip", RobloxNativeType.Instance)]
public partial class AnimationClip : Instance  {
    public bool Loop { get; set; } = default!;
    public Enums.AnimationPriority Priority { get; set; } = default!;
}
