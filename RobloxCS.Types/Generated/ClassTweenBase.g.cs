#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TweenBase", RobloxNativeType.Instance)]
public partial class TweenBase : Instance  {
    public Enums.PlaybackState PlaybackState { get; } = default!;
    public void Cancel() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Pause() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Play() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
