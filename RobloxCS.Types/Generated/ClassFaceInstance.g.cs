#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FaceInstance", RobloxNativeType.Instance)]
public partial class FaceInstance : Instance  {
    public Enums.NormalId Face { get; } = default!;
}
