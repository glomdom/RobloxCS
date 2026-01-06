#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Bone", RobloxNativeType.Instance)]
public partial class Bone : Attachment  {
    public CFrame Transform { get; set; } = default!;
    public CFrame TransformedWorldCFrame { get; set; } = default!;
}
