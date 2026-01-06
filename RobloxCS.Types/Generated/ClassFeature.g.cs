#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Feature", RobloxNativeType.Instance)]
public partial class Feature : Instance  {
    public Enums.NormalId FaceId { get; set; } = default!;
    public Enums.InOut InOut { get; set; } = default!;
    public Enums.LeftRight LeftRight { get; set; } = default!;
    public Enums.TopBottom TopBottom { get; set; } = default!;
}
