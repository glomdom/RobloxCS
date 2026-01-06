#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ShirtGraphic", RobloxNativeType.Instance)]
public partial class ShirtGraphic : CharacterAppearance  {
    public Color3 Color3 { get; set; } = default!;
    public string Graphic { get; set; } = default!;
}
