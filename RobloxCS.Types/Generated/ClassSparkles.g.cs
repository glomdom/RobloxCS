#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Sparkles", RobloxNativeType.Instance)]
public partial class Sparkles : Instance  {
    public bool Enabled { get; } = default!;
    public Color3 SparkleColor { get; } = default!;
    public float TimeScale { get; } = default!;
}
