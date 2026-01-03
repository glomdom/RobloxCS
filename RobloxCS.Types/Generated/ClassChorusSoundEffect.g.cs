#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ChorusSoundEffect", RobloxNativeType.Instance)]
public partial class ChorusSoundEffect : SoundEffect  {
    public float Depth { get; } = default!;
    public float Mix { get; } = default!;
    public float Rate { get; } = default!;
}
