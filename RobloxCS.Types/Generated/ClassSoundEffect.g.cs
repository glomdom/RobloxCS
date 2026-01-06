#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SoundEffect", RobloxNativeType.Instance)]
public partial class SoundEffect : Instance  {
    public bool Enabled { get; set; } = default!;
    public int Priority { get; set; } = default!;
}
