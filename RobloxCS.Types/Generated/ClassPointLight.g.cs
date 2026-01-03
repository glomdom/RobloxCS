#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PointLight", RobloxNativeType.Instance)]
public partial class PointLight : Light  {
    public float Range { get; } = default!;
}
