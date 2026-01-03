#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Feature", RobloxNativeType.Instance)]
public partial class Feature : Instance  {
    public Enums.NormalId FaceId { get; } = default!;
    public Enums.InOut InOut { get; } = default!;
    public Enums.LeftRight LeftRight { get; } = default!;
    public Enums.TopBottom TopBottom { get; } = default!;
}
