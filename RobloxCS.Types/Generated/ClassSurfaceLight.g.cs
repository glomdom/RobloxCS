#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SurfaceLight", RobloxNativeType.Instance)]
public partial class SurfaceLight : Light  {
    public float Angle { get; } = default!;
    public Enums.NormalId Face { get; } = default!;
    public float Range { get; } = default!;
}
