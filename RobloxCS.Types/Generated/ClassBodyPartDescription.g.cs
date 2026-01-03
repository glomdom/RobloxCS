#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BodyPartDescription", RobloxNativeType.Instance)]
public partial class BodyPartDescription : Instance  {
    public int AssetId { get; } = default!;
    public Enums.BodyPart BodyPart { get; } = default!;
    public Color3 Color { get; } = default!;
    public string HeadShape { get; } = default!;
    public Instance Instance { get; } = default!;
}
