#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Sparkles", RobloxNativeType.Instance)]
public partial class Sparkles : Instance  {
    public bool Enabled { get; set; } = default!;
    public Color3 SparkleColor { get; set; } = default!;
    public float TimeScale { get; set; } = default!;
}
