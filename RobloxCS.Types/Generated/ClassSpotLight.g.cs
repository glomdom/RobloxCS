#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SpotLight", RobloxNativeType.Instance)]
public partial class SpotLight : Light  {
    public float Angle { get; } = default!;
    public Enums.NormalId Face { get; } = default!;
    public float Range { get; } = default!;
}
